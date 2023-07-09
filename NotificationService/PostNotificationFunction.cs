using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedKernal;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationService;

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

    [FunctionName("test")]
    public static async Task<IActionResult> TestHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequest request,
        [SignalR(HubName = "serverless")] IAsyncCollector<SignalRMessage> signalRMessages)
    {
        var StarCount = request.Query["id"];

        await signalRMessages.AddAsync(new SignalRMessage
        {
            Target = "newMessage",

            Arguments = new[] { $"Current star count of https://github.com/Azure/azure-signalr is: {StarCount}" }
        });
        return new OkResult();
    }

    [FunctionName(nameof(PostNotificationFunction))]
    public async Task Run(
        [ServiceBusTrigger("post", "notification-service", Connection = "ServiceBus")] EventBusMessageWrapper @event,
        [SignalR(HubName = "serverless")] IAsyncCollector<SignalRMessage> signalRMessages)
    {
        _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {@event}");

        @event.GetType(typeof(PostCreatedIntegrationEvent));

        using var smtpClient = new SmtpClient("localhost", 25);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("admin", "admin");
        smtpClient.EnableSsl = false;

        var message = new MailMessage
        {
            From = new MailAddress("info@facebook.com"),
            IsBodyHtml = true,
        };

        string authorId = "";
        var type = @event.GetType(typeof(PostCreatedIntegrationEvent));
        if (type == typeof(PostCreatedIntegrationEvent))
        {
            var data = @event.Convert<PostCreatedIntegrationEvent>();
            authorId = data.Author.Id;
            message.Subject = $"{data.Author.Name} added a post";
            message.Body = data.Summary;
        }
        else if (type == typeof(PostCommentedIntegrationEvent))
        {
            var data = @event.Convert<PostCommentedIntegrationEvent>();
            authorId = data.Author.Id;
            message.Subject = $"{data.Author.Name} commented on a post";
            message.Body = data.Summary;
        }

        var friends = await GetAuthorFriendsAsync(authorId);
        var notifications = new List<Task>();
        notifications.Add(signalRMessages.AddAsync(new SignalRMessage
        {
            Target = authorId,
            Arguments = new[] { message.Subject }
        }));
        foreach (var friend in friends)
        {
            message.To.Add(new MailAddress(friend.Email));
            notifications.Add(signalRMessages.AddAsync(new SignalRMessage
            {
                Target = friend.UserId,
                Arguments = new[] { message.Subject }
            }));
        }
        smtpClient.Send(message);
        await Task.WhenAll(notifications);
    }

    private async Task<List<Friend>> GetAuthorFriendsAsync(string authorId)
    {
        if (string.IsNullOrEmpty(authorId)) return default;

        var friendsResponse = await _httpClient.GetAsync($"{_configuration["FriendshipService"]}?userId={authorId}");
        var friends = await friendsResponse.Content.ReadAsAsync<List<Friend>>();
        return friends;
    }
}


public record Friend
{
    public string Email { get; init; }
    public string Name { get; init; }
    public string UserId { get; init; }
}


public record PostCreatedIntegrationEvent : BaseEvent<string>
{
    public AuthorEventData Author { get; init; }
    public string Summary { get; set; }
}

public record PostCommentedIntegrationEvent : BaseEvent<string>
{
    public AuthorEventData Author { get; init; }
    public string Summary { get; set; }
}

public record AuthorEventData
{
    public string Id { get; init; }
    public string Name { get; init; }
}
