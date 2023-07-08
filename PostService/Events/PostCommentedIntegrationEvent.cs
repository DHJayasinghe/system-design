using PostService.Models;
using SharedKernal;

namespace PostService.Events;

public record PostCommentedIntegrationEvent : BaseEvent<string>
{
    public AuthorEventData Author { get; init; }
    public string Summary { get; set; }

    public PostCommentedIntegrationEvent(Comment entity)
    {
        Id = entity.PostId;
        Author = new AuthorEventData
        {
            Id = entity.AuthorId,
            Name = entity.AuthorName
        };
        Summary = entity.Content.Length > 200 ? entity.Content[..200] : entity.Content;
    }
}
