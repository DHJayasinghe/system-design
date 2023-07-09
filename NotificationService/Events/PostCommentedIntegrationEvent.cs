using SharedKernal;

namespace NotificationService.Events;

public record PostCommentedIntegrationEvent : BaseEvent<string>
{
    public AuthorEventData Author { get; init; }
    public string Summary { get; init; }
}