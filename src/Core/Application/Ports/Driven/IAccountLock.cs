using Me.Acheddir.Hexagonal.SharedKernel.Ports.Driven;

namespace Me.Acheddir.Hexagonal.Application.Ports.Driven;

public interface IAccountLock : IDrivenPort
{
    Task LockAccountAsync(AccountId accountId, CancellationToken token);
    Task ReleaseAccountAsync(AccountId accountId, CancellationToken token);
}
