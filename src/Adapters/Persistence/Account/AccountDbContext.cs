namespace Me.Acheddir.Hexagonal.Persistence.Account;

public class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }
    
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
}