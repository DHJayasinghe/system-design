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
    public int LikeCount { get; set; } = 0;
    public int HeartCount { get; set; } = 0;
    public int WowCount { get; set; } = 0;
    public int CareCount { get; set; } = 0;
    public int LaughCount { get; set; } = 0;
    public int SadCount { get; set; } = 0;
    public int AngryCount { get; set; } = 0;
    public DateTime Timestamp { get; set; }

    public ReactionCount WithDefaults()
    {
        Id = $"{CommentId ?? PostId}";
        Timestamp = DateTime.UtcNow;
        return this;
    }

    public void Decrement(ReactionType reaction)
    {
        if (reaction == ReactionType.LIKE)
            LikeCount--;
        else if (reaction == ReactionType.HEART)
            HeartCount--;
        else if (reaction == ReactionType.CARE)
            CareCount--;
        else if (reaction == ReactionType.LAUGH)
            LaughCount--;
        else if (reaction == ReactionType.WOW)
            WowCount--;
        else if (reaction == ReactionType.SAD)
            SadCount--;
        else if (reaction == ReactionType.ANGRY)
            AngryCount--;
    }

    public void Increment(ReactionType reaction)
    {
        if (reaction == ReactionType.LIKE)
            LikeCount++;
        else if (reaction == ReactionType.HEART)
            HeartCount++;
        else if (reaction == ReactionType.CARE)
            CareCount++;
        else if (reaction == ReactionType.LAUGH)
            LaughCount++;
        else if (reaction == ReactionType.WOW)
            WowCount++;
        else if (reaction == ReactionType.SAD)
            SadCount++;
        else if (reaction == ReactionType.ANGRY)
            AngryCount++;
    }

    public static ReactionCount Map(ReactionChangedIntegrationEvent @event)
    {
        return new ReactionCount()
        {
            PostId = @event.PostId,
            CommentId = @event.CommentId,
        }.WithDefaults();
    }

    public static ReactionCount Map(Reaction reaction)
    {
        return new ReactionCount()
        {
            PostId = reaction.PostId,
            CommentId = reaction.CommentId,
        }.WithDefaults();
    }
}