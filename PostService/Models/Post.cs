using Newtonsoft.Json;
using PostService.API.Models;
***REMOVED***
***REMOVED***

namespace PostService.Models;

public record Post
***REMOVED***
    [JsonProperty("id")]
    public string Id ***REMOVED*** get; set; ***REMOVED***
    public string PostId ***REMOVED*** get; set; ***REMOVED***
    public string AuthorId ***REMOVED*** get; set; ***REMOVED***
    public string AuthorName ***REMOVED*** get; set; ***REMOVED*** = "Dhanuka Jayasinghe";
    public string Content ***REMOVED*** get; set; ***REMOVED***
    public List<string> Assets ***REMOVED*** get; set; ***REMOVED***
    public DateTime CreatedAt ***REMOVED*** get; set; ***REMOVED*** = DateTime.UtcNow;
    public DateTime? UpdatedAt ***REMOVED*** get; set; ***REMOVED***

    public static Post Map(AddPostRequest request)
    ***REMOVED***
        var id = Guid.NewGuid().ToString();
        return new()
        ***REMOVED***
            Id = id,
            PostId = id,
            Content = request.Content,
            Assets = request.Assets,
    ***REMOVED***;
***REMOVED***
***REMOVED***