using System.Collections.Generic;

namespace PostService.API.Models;

public record AddPostRequest
{
    public string Content { get; init; }
    public List<string> Assets { get; init; }
}
