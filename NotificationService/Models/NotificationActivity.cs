using Azure;
using Azure.Data.Tables;
using System;

namespace NotificationService.Models;

public record NotificationActivity : BaseTableEntity
{
    public string Key => $"{OwnerId}-{Year}{Month}";
    public string OwnerId { get; init; }
    public string Content { get; init; }
    public int Month { get; private init; } = DateTime.UtcNow.Month;
    public int Year { get; private init; } = DateTime.UtcNow.Year;
    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

    public static string GetKey(string ownerId) => $"{ownerId}-{ DateTime.UtcNow.Year}{DateTime.UtcNow.Month}";
}

public record BaseTableEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

