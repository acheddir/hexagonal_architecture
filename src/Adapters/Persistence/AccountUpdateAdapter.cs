namespace Me.Acheddir.Hexagonal.Persistence;

public class AccountUpdateAdapter : IAccountUpdate
{
    public Task UpdateActivitiesAsync(Account account, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}