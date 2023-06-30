using System;

namespace PostService.Models;

public record Timeline
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Key => $"{OwnerId}-{Year}{Month}";
    public string OwnerId { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
    public string PostId { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}