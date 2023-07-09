using System;

namespace NotificationService.Models;

public record FriendResponse
{
    public string Email { get; init; }
    public string Name { get; init; }
    public string UserId { get; init; }
}

public record NotificationActivityResponse
{
    public string Content { get; init; }
    public DateTime CreatedAt { get; init; }
    public NotificationActivityResponse(NotificationActivity activity)
    {
        Content = activity.Content;
        CreatedAt = activity.CreatedAt;
    }
}