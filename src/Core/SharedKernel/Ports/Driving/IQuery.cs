namespace SharedKernel.Ports.Driving;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
