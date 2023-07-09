using SharedKernal;

namespace NotificationService.Events;

public record PostCreatedIntegrationEvent : BaseEvent<string>
{
    public AuthorEventData Author { get; init; }
    public string Summary { get; init; }
}
