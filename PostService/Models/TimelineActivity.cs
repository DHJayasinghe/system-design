using Newtonsoft.Json;
using System;

namespace PostService.Models;

public record TimelineActivity
{
    [JsonProperty("id")]
    public string Id { get; private init; } = Guid.NewGuid().ToString();
    public string Key => $"{OwnerId}-{Year}{Month}";
    public string OwnerId { get; init; }
    public string PostId { get; init; }
    public int Month { get; private init; } = DateTime.UtcNow.Month;
    public int Year { get; private init; } = DateTime.UtcNow.Year;
    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

    public static string GetKey(string ownerId) => $"{ownerId}-{ DateTime.UtcNow.Year}{DateTime.UtcNow.Month}";
}