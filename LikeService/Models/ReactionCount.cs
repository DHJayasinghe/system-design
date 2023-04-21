using LikeService.Events;
using Newtonsoft.Json;
***REMOVED***

namespace LikeService.Models;

public record ReactionCount
***REMOVED***
    [JsonProperty("id")]
    public string Id ***REMOVED*** get; set; ***REMOVED***
    public string PostId ***REMOVED*** get; init; ***REMOVED***
    public string CommentId ***REMOVED*** get; init; ***REMOVED***
    public ReactionType ReactionType ***REMOVED*** get; init; ***REMOVED***
    public DateTime Timestamp ***REMOVED*** get; set; ***REMOVED***
    public int Count ***REMOVED*** get; set; ***REMOVED*** = 0;

    public ReactionCount WithDefaults()
    ***REMOVED***
        Id = $"***REMOVED***CommentId ?? PostId***REMOVED******REMOVED***ReactionType***REMOVED***";
        Timestamp = DateTime.UtcNow;
        return this;
***REMOVED***

    public static ReactionCount Map(ReactionChangedIntegrationEvent @event, ReactionType? reactionType = null)
    ***REMOVED***
        return new ReactionCount()
        ***REMOVED***
            PostId = @event.PostId,
            CommentId = @event.CommentId,
            ReactionType = reactionType ?? @event.ReactionType
    ***REMOVED***.WithDefaults();
***REMOVED***
***REMOVED***
