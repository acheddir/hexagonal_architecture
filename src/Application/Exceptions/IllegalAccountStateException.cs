namespace Me.Acheddir.Hexagonal.Application.Exceptions;

[Serializable]
public class IllegalAccountStateException : Exception
{
    public IllegalAccountStateException(string message)
        : base(message)
    {
    }

    public IllegalAccountStateException(string message, Exception exception)
        : base(message, exception)
    {
    }

    protected IllegalAccountStateException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
