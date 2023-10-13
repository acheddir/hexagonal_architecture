namespace Me.Acheddir.Hexagonal.Persistence.Account;

public interface IAccountUnitOfWork : IUnitOfWork
{
}

public class AccountUnitOfWork : IAccountUnitOfWork
{
    private readonly AccountDbContext _context;

    public AccountUnitOfWork(AccountDbContext context)
    {
        _context = context;
    }

    public Task<int> Commit()
    {
        return _context.SaveChangesAsync();
    }
}