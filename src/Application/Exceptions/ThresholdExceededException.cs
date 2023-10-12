namespace Me.Acheddir.Hexagonal.Application.Exceptions;

[Serializable]
public class ThresholdExceededException : Exception
{
    public ThresholdExceededException(Money threshold, Money actual)
        : base(
            $"Maximum threshold for transferring money exceeded: tried to transfer {actual} but threshold is {threshold}")
    {
    }

    public ThresholdExceededException(string message)
        : base(message)
    {
    }

    public ThresholdExceededException(string message, Exception exception)
        : base(message, exception)
    {
    }

    protected ThresholdExceededException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
