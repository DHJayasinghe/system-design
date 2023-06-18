using Newtonsoft.Json;
using PostService.API.Models;
using System;
using System.Collections.Generic;

namespace PostService.Models;

public record Post
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string PostId { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; } = "Dhanuka Jayasinghe";
    public string Content { get; set; }
    public List<string> Assets { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public static Post Map(AddPostRequest request)
    {
        var id = Guid.NewGuid().ToString();
        return new()
        {
            Id = id,
            PostId = id,
            Content = request.Content,
            Assets = request.Assets,
        };
    }
}