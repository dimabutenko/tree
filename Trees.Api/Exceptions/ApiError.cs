namespace Trees.Exceptions;

public class ApiError
{
    public ApiError(string id, string type, string message)
    {
        Id = id;
        Type = type;
        Data = new ApiErrorData { Message = message };
    }

    public ApiError(string id) : this(id, nameof(Exception), $"Internal server error Id={id}") { }
    public ApiError(string id, string type, Exception exception) : this(id, type, exception.Message) { }

    public string? Id { get; }
    public string? Type { get; }
    public ApiErrorData Data { get; }
}

public class ApiErrorData
{
    public string Message { get; set; }
}
