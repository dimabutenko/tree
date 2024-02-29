using System.Runtime.Serialization;

namespace Tree.Services.Exceptions;

[Serializable]
public class DatabaseUpdateException : Exception
{
    public DatabaseUpdateException(string message, Exception inner) : base(message, inner) { }

    protected DatabaseUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
