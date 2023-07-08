namespace SharedKernal;

public record BaseEvent<T>
{
    public DateTimeOffset DateOccurred { get; private init; } = DateTimeOffset.UtcNow;
    public T Id { get; init; }
}
