using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace LikeService.Models;

public record Reaction
{
    private static readonly Regex regex = new("[^a-zA-Z0-9]");

    [JsonProperty("id")]
    public string Id { get; set; }
    public string PostId { get; set; }
    public string CommentId { get; set; }
    public string UserId { get; set; }
    public LikeType LikeType { get; set; }
    public DateTime Timestamp { get; set; }

    public Reaction WithDefaults()
    {
        Id = regex.Replace($"{PostId}{CommentId ?? ""}{UserId}", string.Empty);
        Timestamp = DateTime.UtcNow;
        return this;
    }
}

public enum LikeType
{
    LIKE = 0,
    HEART = 1,
    CARE = 2,
    LAUGH = 3,
    WOW = 4,
    SAD = 5,
    ANGRY = 6
}
