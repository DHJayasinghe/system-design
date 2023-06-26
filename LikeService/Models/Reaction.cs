using Newtonsoft.Json;
***REMOVED***
using System.Text.RegularExpressions;

namespace LikeService.Models;

public record Reaction
***REMOVED***
    private static readonly Regex regex = new("[^a-zA-Z0-9]");

    [JsonProperty("id")]
    public string Id ***REMOVED*** get; set; ***REMOVED***
    public string PostId ***REMOVED*** get; set; ***REMOVED***
    public string CommentId ***REMOVED*** get; set; ***REMOVED***
    public string UserId ***REMOVED*** get; set; ***REMOVED***
    public ReactionType ReactionType ***REMOVED*** get; set; ***REMOVED***
    public DateTime Timestamp ***REMOVED*** get; set; ***REMOVED***

    public Reaction WithDefaults()
***REMOVED***
        Id = regex.Replace($"***REMOVED***CommentId ?? PostId***REMOVED******REMOVED***UserId***REMOVED***", string.Empty);
        Timestamp = DateTime.UtcNow;
        return this;
    ***REMOVED***
***REMOVED***

public enum ReactionType
***REMOVED***
    LIKE = 0,
    HEART = 1,
    CARE = 2,
    LAUGH = 3,
    WOW = 4,
    SAD = 5,
    ANGRY = 6
***REMOVED***
