using System.Runtime.Serialization;

namespace Tree.Services.Exceptions;

[Serializable]
public class TreeNodeException : Exception
{
    public TreeNodeException(string message) : base(message) { }
    public TreeNodeException(string message, Exception inner) : base(message, inner) { }

    protected TreeNodeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
