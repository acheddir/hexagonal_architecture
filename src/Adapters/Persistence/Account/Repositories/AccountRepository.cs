namespace Me.Acheddir.Hexagonal.Persistence.Account.Repositories;

public interface IAccountRepository : IRepository
{
    Task<AccountEntity?> GetAccountById(long accountIdId, CancellationToken token);
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
}