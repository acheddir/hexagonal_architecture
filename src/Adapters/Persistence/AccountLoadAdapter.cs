namespace Me.Acheddir.Hexagonal.Persistence;

public class AccountLoadAdapter : IAccountLoad
{
    public Task<Account> LoadAccountAsync(AccountId requestAccountId, DateTime utcNow, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}