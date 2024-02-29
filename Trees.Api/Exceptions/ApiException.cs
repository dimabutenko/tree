using System.Runtime.Serialization;

namespace Trees.Exceptions;

[Serializable]
public sealed class ApiException : Exception
{
    public ApiException(Exception exception) : base(exception.Message)
    {
        Id = "";
        Type = exception.GetType().Name;
    }

    private ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Id = info.GetString(nameof(Id));
        Type = info.GetString(nameof(Type));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ArgumentNullException.ThrowIfNull(info);

        info.AddValue(nameof(Id), Id);
        info.AddValue(nameof(Type), Type);

        base.GetObjectData(info, context);
    }

    public string? Id { get; }
    public string? Type { get; }
}
