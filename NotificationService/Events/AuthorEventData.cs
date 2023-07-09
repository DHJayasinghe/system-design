namespace NotificationService.Events;

public record AuthorEventData
{
    public string Id { get; init; }
    public string Name { get; init; }
}
