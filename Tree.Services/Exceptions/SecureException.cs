using System.Runtime.Serialization;

namespace Tree.Services.Exceptions;

[Serializable]
public class SecureException : Exception
{
    public SecureException(string message) : base(message) { }
    public SecureException(string message, Exception inner) : base(message, inner) { }

    protected SecureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
