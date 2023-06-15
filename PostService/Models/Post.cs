using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PostService.Models
{
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

        public static Post Map(PostRequest request)
        {
            return new Post
            {
                Id = request.PostId.ToString(),
                PostId = request.PostId.ToString(),
                Content = request.Content,
                Assets = request.Assets,
            };
        }
    }
}
