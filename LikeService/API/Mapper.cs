using LikeService.Models;

namespace LikeService.API;

public static class Mapper
{
    public static Reaction Map(this AddReactionRequest data) => new Reaction
    {
        PostId = data.PostId,
        CommentId = data.CommentId,
        UserId = data.UserId,
        ReactionType = (ReactionType)data.ReactionType
    };

    public static Reaction Map(this RemoveReactionRequest data) => new Reaction
    {
        PostId = data.PostId,
        CommentId = data.CommentId,
        UserId = data.UserId,
    };
}