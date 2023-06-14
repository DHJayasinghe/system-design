using Newtonsoft.Json;
using System.Collections.Generic;

namespace PostService.Models
{
    public record Post
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public List<string> Assets { get; set; }

        public static Post Map(PostRequest request)
        {
            return new Post
            {
                Id = request.PostId.ToString(),
                PostId = request.PostId.ToString(),
                Description = request.Description,
                Assets = request.Assets,
            };
        }
    }
}
