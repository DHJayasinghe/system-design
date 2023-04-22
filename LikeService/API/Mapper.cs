using LikeService.Models;

namespace LikeService.API;

public static class M***REMOVED***er
***REMOVED***
    public static Reaction Map(this AddReactionRequest data) => new Reaction
    ***REMOVED***
        PostId = data.PostId,
        CommentId = data.CommentId,
        UserId = data.UserId,
        ReactionType = (ReactionType)data.ReactionType
***REMOVED***;

    public static Reaction Map(this RemoveReactionRequest data) => new Reaction
    ***REMOVED***
        PostId = data.PostId,
        CommentId = data.CommentId,
        UserId = data.UserId,
***REMOVED***;
***REMOVED***