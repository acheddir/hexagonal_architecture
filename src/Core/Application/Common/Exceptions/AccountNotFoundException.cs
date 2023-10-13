namespace Me.Acheddir.Hexagonal.Application.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(long id)
        : base($"Account [{id}] not found.")
    {
    }
    
    public AccountNotFoundException(string message)
        : base(message)
    {
    }

    public AccountNotFoundException(string message, Exception exception)
        : base(message, exception)
    {
    }

    protected AccountNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}