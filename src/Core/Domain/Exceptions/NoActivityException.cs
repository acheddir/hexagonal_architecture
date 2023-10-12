namespace Me.Acheddir.Hexagonal.Domain.Exceptions;

public class NoActivityException : Exception
{
    public NoActivityException(string? message) : base(message)
    {
    }

    public NoActivityException(string? message, Exception? exception) : base(message, exception)
    {
    }

    public NoActivityException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
