namespace Me.Acheddir.Hexagonal.Application.Ports.Driven;

public interface IAccountLock
{
    Task LockAccountAsync(AccountId accountId, CancellationToken token);
    Task ReleaseAccountAsync(AccountId accountId, CancellationToken token);
}
