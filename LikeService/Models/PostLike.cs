using Newtonsoft.Json;
using System;

namespace LikeService.Models
{
    public record PostLike
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid? CommentId { get; set; }
        public string UserId { get; set; }
        public LikeType LikeType { get; set; }
        public DateTime Timestamp { get; set; }

        public void AddDefaults()
        {
            Id = PostId;
            Timestamp = DateTime.UtcNow;
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
}
