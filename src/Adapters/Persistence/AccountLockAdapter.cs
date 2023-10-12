namespace Me.Acheddir.Hexagonal.Persistence;

public class AccountLockAdapter : IAccountLock
{
    public Task LockAccountAsync(AccountId accountId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task ReleaseAccountAsync(AccountId accountId, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}