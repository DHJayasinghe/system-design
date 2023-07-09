using PostService.Models;
using SharedKernal;

namespace PostService.Events;

public record PostCreatedIntegrationEvent : BaseEvent<string>
{
    public AuthorEventData Author { get; init; }
    public string Summary { get; set; }

    public PostCreatedIntegrationEvent(Post post)
    {
        Id = post.PostId;
        Author = new AuthorEventData
        {
            Id = post.AuthorId,
            Name = post.AuthorName
        };
        Summary = post.Content.Length > 200 ? post.Content[..200] : post.Content;
    }
}

public record AuthorEventData
{
    public string Id { get; init; }
    public string Name { get; init; }
}
