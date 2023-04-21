using LikeService.Models;
***REMOVED***

namespace LikeService.Events;

public record ReactionChangedIntegrationEvent : IntegrationEvent
***REMOVED***
    public string PostId ***REMOVED*** get; init; ***REMOVED***
    public string CommentId ***REMOVED*** get; init; ***REMOVED***
    public string UserId ***REMOVED*** get; init; ***REMOVED***
    public ReactionType? PreviousReactionType ***REMOVED*** get; set; ***REMOVED***
    public ReactionType ReactionType ***REMOVED*** get; init; ***REMOVED***
    public State State ***REMOVED*** get; init; ***REMOVED*** = State.Added;
    
***REMOVED***

public enum State
***REMOVED***
    Added = 0,
    Modified = 1,
    Removed = 2
***REMOVED***

public abstract record IntegrationEvent
***REMOVED***
    public string Id ***REMOVED*** get; set; ***REMOVED***
    public DateTimeOffset DateOccurred ***REMOVED*** get; set; ***REMOVED*** = DateTimeOffset.UtcNow;
***REMOVED***