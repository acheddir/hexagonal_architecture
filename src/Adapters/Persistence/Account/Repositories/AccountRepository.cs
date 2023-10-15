namespace Me.Acheddir.Hexagonal.Persistence.Account.Repositories;

public interface IAccountRepository : IRepository
{
    Task<AccountEntity?> GetAccountById(long accountIdId, CancellationToken token);
    void Save(ActivityEntity activityEntity);
}

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _context;

    public AccountRepository(AccountDbContext context)
    {
        _context = context;
    }

    public Task<AccountEntity?> GetAccountById(long accountId, CancellationToken token)
    {
        return _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken: token);
    }

    public void Save(ActivityEntity activityEntity)
    {
        _context.Activities.Add(activityEntity);
    }
}