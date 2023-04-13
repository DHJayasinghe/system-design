using Newtonsoft.Json;
using System;

namespace UrlShortenerService.Models;

public record ShortenedUrl
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string Value { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
}