using Newtonsoft.Json;
using System;

namespace LikeService.Models;

public record ReactionCount
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string PostId { get; set; }
    public string CommentId { get; set; }
    public LikeType LikeType { get; set; }
    public DateTime Timestamp { get; set; }
    public int Count { get; set; } = 0;
}
