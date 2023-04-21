using LikeService.Events;
using Newtonsoft.Json;
using System;

namespace LikeService.Models;

public record ReactionCount
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string PostId { get; init; }
    public string CommentId { get; init; }
    public ReactionType ReactionType { get; init; }
    public DateTime Timestamp { get; set; }
    public int Count { get; set; } = 0;

    public ReactionCount WithDefaults()
    {
        Id = $"{CommentId ?? PostId}{ReactionType}";
        Timestamp = DateTime.UtcNow;
        return this;
    }

    public static ReactionCount Map(ReactionChangedIntegrationEvent @event, ReactionType? reactionType = null)
    {
        return new ReactionCount()
        {
            PostId = @event.PostId,
            CommentId = @event.CommentId,
            ReactionType = reactionType ?? @event.ReactionType
        }.WithDefaults();
    }
}
