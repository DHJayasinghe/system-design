using Newtonsoft.Json;
***REMOVED***

namespace UrlShortenerService.Models;

public record ShortenedUrl
***REMOVED***
    [JsonProperty("id")]
    public string Id ***REMOVED*** get; set; ***REMOVED***
    public string Value ***REMOVED*** get; set; ***REMOVED***
    public DateTime CreatedDateTime ***REMOVED*** get; set; ***REMOVED*** = DateTime.UtcNow;
***REMOVED***