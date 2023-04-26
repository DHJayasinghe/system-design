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
    public int LikeCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int HeartCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int WowCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int CareCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int LaughCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int SadCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int AngryCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public DateTime Timestamp ***REMOVED*** get; set; ***REMOVED***

    public ReactionCount WithDefaults()
    ***REMOVED***
        Id = $"***REMOVED***CommentId ?? PostId***REMOVED***";
        Timestamp = DateTime.UtcNow;
        return this;
***REMOVED***

    public void Decrement(ReactionType reaction)
    ***REMOVED***
        if (reaction == ReactionType.LIKE)
            LikeCount--;
        else if (reaction == ReactionType.HEART)
            HeartCount--;
        else if (reaction == ReactionType.CARE)
            CareCount--;
        else if (reaction == ReactionType.LAUGH)
            LaughCount--;
        else if (reaction == ReactionType.WOW)
            WowCount--;
        else if (reaction == ReactionType.SAD)
            SadCount--;
        else if (reaction == ReactionType.ANGRY)
            AngryCount--;
***REMOVED***

    public void Increment(ReactionType reaction)
    ***REMOVED***
        if (reaction == ReactionType.LIKE)
            LikeCount++;
        else if (reaction == ReactionType.HEART)
            HeartCount++;
        else if (reaction == ReactionType.CARE)
            CareCount++;
        else if (reaction == ReactionType.LAUGH)
            LaughCount++;
        else if (reaction == ReactionType.WOW)
            WowCount++;
        else if (reaction == ReactionType.SAD)
            SadCount++;
        else if (reaction == ReactionType.ANGRY)
            AngryCount++;
***REMOVED***

    public static ReactionCount Map(ReactionChangedIntegrationEvent @event)
    ***REMOVED***
        return new ReactionCount()
        ***REMOVED***
            PostId = @event.PostId,
            CommentId = @event.CommentId,
    ***REMOVED***.WithDefaults();
***REMOVED***

    public static ReactionCount Map(Reaction reaction)
    ***REMOVED***
        return new ReactionCount()
        ***REMOVED***
            PostId = reaction.PostId,
            CommentId = reaction.CommentId,
    ***REMOVED***.WithDefaults();
***REMOVED***
***REMOVED***