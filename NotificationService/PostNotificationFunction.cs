using Microsoft.Azure.WebJobs;
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

    [FunctionName(nameof(PostNotificationFunction))]
    public async Task Run([ServiceBusTrigger("post", "notification-service", Connection = "ServiceBus")] EventBusMessageWrapper @event)
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
        foreach (var friend in friends)
        {
            message.To.Add(new MailAddress(friend.Email));
        }
        smtpClient.Send(message);
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
