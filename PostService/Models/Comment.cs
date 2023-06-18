using Newtonsoft.Json;
using PostService.API;
using System;

namespace PostService.Models;

public record Comment
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string CommentId { get; set; }
    public string PostId { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; } = "Dhanuka Jayasinghe";
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public static Comment Map(Guid postId, AddCommentRequest request)
    {
        var id = Guid.NewGuid().ToString();
        return new()
        {
            Id = id,
            CommentId = id,
            PostId = postId.ToString(),
            Content = request.Content,
        };
    }
}
