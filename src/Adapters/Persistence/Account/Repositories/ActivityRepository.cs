namespace Me.Acheddir.Hexagonal.Persistence.Account.Repositories;

public interface IActivityRepository : IRepository
{
    Task<List<ActivityEntity>>
        GetAccountActivitiesSince(long accountId, DateTime baselineDate, CancellationToken token);

    Task<long> GetWithdrawalBalanceUntil(long accountId, DateTime baselineDate, CancellationToken token);
    Task<long> GetDepositBalanceUntil(long accountId, DateTime baselineDate, CancellationToken token);
}

public class ActivityRepository : IActivityRepository
{
    private readonly AccountDbContext _context;

    public ActivityRepository(AccountDbContext context)
    {
        _context = context;
    }

    public Task<List<ActivityEntity>> GetAccountActivitiesSince(long accountId, DateTime baselineDate,
        CancellationToken token)
    {
        return _context.Activities
            .Where(a => a.OwnerAccountId == accountId &&
                        a.Timestamp >= baselineDate)
            .ToListAsync(token);
    }

    public Task<long> GetWithdrawalBalanceUntil(long accountId, DateTime baselineDate, CancellationToken token)
    {
        return _context.Activities
            .Where(a => a.SourceAccountId == accountId &&
                        a.OwnerAccountId == accountId &&
                        a.Timestamp < baselineDate)
            .SumAsync(a => a.Amount, cancellationToken: token);
    }

    public Task<long> GetDepositBalanceUntil(long accountId, DateTime baselineDate, CancellationToken token)
    {
        return _context.Activities
            .Where(a => a.TargetAccountId == accountId &&
                        a.OwnerAccountId == accountId &&
                        a.Timestamp < baselineDate)
            .SumAsync(a => a.Amount, cancellationToken: token);
    }
}
