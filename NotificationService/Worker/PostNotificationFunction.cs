using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Events;
using NotificationService.Models;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationService.Worker;

public class PostNotificationFunction
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PostNotificationFunction> _logger;

    public PostNotificationFunction(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<PostNotificationFunction> log)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
        _logger = log;
    }

    [FunctionName("negotiate")]
    public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notifications/negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "serverless")] SignalRConnectionInfo connectionInfo) => connectionInfo;

    [FunctionName(nameof(PostNotificationFunction))]
    public async Task Run(
        [ServiceBusTrigger("post", "notification-service", Connection = "ServiceBus")] EventBusMessageWrapper @event,
        [Table("notification")] IAsyncCollector<NotificationActivity> activities,
        [SignalR(HubName = "serverless")] IAsyncCollector<SignalRMessage> signalRMessages)
    {
        _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {@event}");

        @event.GetType(typeof(PostCreatedIntegrationEvent));

        string authorId = "";
        string title = "";
        string summary = "";
        var type = @event.GetType(typeof(PostCreatedIntegrationEvent));
        if (type == typeof(PostCreatedIntegrationEvent))
        {
            var data = @event.Convert<PostCreatedIntegrationEvent>();
            authorId = data.Author.Id;
            title = $"{data.Author.Name} added a post";
            summary = data.Summary;
        }
        else if (type == typeof(PostCommentedIntegrationEvent))
        {
            var data = @event.Convert<PostCommentedIntegrationEvent>();
            authorId = data.Author.Id;
            title = $"{data.Author.Name} commented on a post";
            summary = data.Summary;
        }
        else return;

        var friends = await GetAuthorFriendsAsync(authorId);
        await CreateNotificationActivitiesAsync(activities, title, friends);
        await SendEmailNotificationAsync(title, summary, friends.Select(friend => friend.Email));
        await SendPushNotificationAsync(signalRMessages, title, friends.Select(d => d.UserId));
    }

    private static async Task CreateNotificationActivitiesAsync(IAsyncCollector<NotificationActivity> activities, string title, List<FriendResponse> friends)
    {
        var activitiesCreateTasks = new List<Task>();
        foreach (var friend in friends)
        {
            var key = NotificationActivity.GetKey(friend.UserId);
            activitiesCreateTasks.Add(activities.AddAsync(new NotificationActivity
            {
                OwnerId = friend.UserId,
                Content = title,
                PartitionKey = key,
                RowKey = Guid.NewGuid().ToString()
            }));
        }
        await Task.WhenAll(activitiesCreateTasks);
    }

    private static async Task SendPushNotificationAsync(IAsyncCollector<SignalRMessage> signalRMessages, string title, IEnumerable<string> receiverIds)
    {
        var notifications = new List<Task>();
        foreach (var receiverId in receiverIds)
        {
            notifications.Add(signalRMessages.AddAsync(new SignalRMessage
            {
                Target = receiverId,
                Arguments = new[] { title }
            }));
        }

        await Task.WhenAll(notifications);
    }

    private static async Task SendEmailNotificationAsync(string title, string summary, IEnumerable<string> receivers)
    {
        using var smtpClient = new SmtpClient("localhost", 25);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("admin", "admin");
        smtpClient.EnableSsl = false;

        var message = new MailMessage
        {
            From = new MailAddress("info@facebook.com"),
            IsBodyHtml = true,
        };
        message.Subject = title;
        message.Body = summary;
        foreach (var receiver in receivers)
        {
            message.To.Add(new MailAddress(receiver));
        }
        await smtpClient.SendMailAsync(message);
    }

    private async Task<List<FriendResponse>> GetAuthorFriendsAsync(string authorId)
    {
        if (string.IsNullOrEmpty(authorId)) return default;

        var friendsResponse = await _httpClient.GetAsync($"{_configuration["FriendshipService"]}?userId={authorId}");
        var friends = await friendsResponse.Content.ReadAsAsync<List<FriendResponse>>();
        return friends;
    }
}