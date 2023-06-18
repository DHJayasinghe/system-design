namespace PostService.API.Models;

public record AddCommentRequest
{
    public string Content { get; init; }
}
