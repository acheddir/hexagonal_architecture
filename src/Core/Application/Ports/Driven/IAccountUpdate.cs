using Me.Acheddir.Hexagonal.SharedKernel.Ports.Driven;

namespace Me.Acheddir.Hexagonal.Application.Ports.Driven;

public interface IAccountUpdate : IDrivenPort
{
    Task UpdateActivitiesAsync(Account account, CancellationToken token);
}
