namespace Trees.Data.Entities;

public class LogEntity
{
    public Guid Id { get; init; }
    public string TraceId { get; init; }
    public string Text { get; init; }
    public DateTime CreatedAt { get; init; }
}
