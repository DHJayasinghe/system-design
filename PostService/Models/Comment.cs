using Newtonsoft.Json;
using PostService.API.Models;
***REMOVED***

namespace PostService.Models;

public record Comment
***REMOVED***
    [JsonProperty("id")]
    public string Id ***REMOVED*** get; set; ***REMOVED***
    public string CommentId ***REMOVED*** get; set; ***REMOVED***
    public string PostId ***REMOVED*** get; set; ***REMOVED***
    public string AuthorId ***REMOVED*** get; set; ***REMOVED***
    public string AuthorName ***REMOVED*** get; set; ***REMOVED*** = "Dhanuka Jayasinghe";
    public string Content ***REMOVED*** get; set; ***REMOVED***
    public DateTime CreatedAt ***REMOVED*** get; set; ***REMOVED*** = DateTime.UtcNow;
    public DateTime? UpdatedAt ***REMOVED*** get; set; ***REMOVED***

    public static Comment Map(Guid postId, AddCommentRequest request)
***REMOVED***
        var id = Guid.NewGuid().ToString();
        return new()
    ***REMOVED***
            Id = id,
            CommentId = id,
            PostId = postId.ToString(),
            Content = request.Content,
***REMOVED***
    ***REMOVED***
***REMOVED***
