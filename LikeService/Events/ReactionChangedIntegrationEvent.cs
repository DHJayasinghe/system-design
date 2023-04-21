using LikeService.Models;
using System;

namespace LikeService.Events;

public record ReactionChangedIntegrationEvent : IntegrationEvent
{
    public string PostId { get; init; }
    public string CommentId { get; init; }
    public string UserId { get; init; }
    public ReactionType? PreviousReactionType { get; set; }
    public ReactionType ReactionType { get; init; }
    public State State { get; init; } = State.Added;
    
}

public enum State
{
    Added = 0,
    Modified = 1,
    Removed = 2
}

public abstract record IntegrationEvent
{
    public string Id { get; set; }
    public DateTimeOffset DateOccurred { get; set; } = DateTimeOffset.UtcNow;
}