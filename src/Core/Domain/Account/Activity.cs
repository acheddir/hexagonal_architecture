namespace Me.Acheddir.Hexagonal.Domain.Account;

public class Activity : Entity<ActivityId>
{
    private AccountId? OwnerAccountId { get; }
    public AccountId? SourceAccountId { get; }
    public AccountId? TargetAccountId { get; }
    public DateTime Timestamp { get; }
    public Money Money { get; }
    
    public Activity(AccountId? ownerAccountId, AccountId? sourceAccountId, AccountId? targetAccountId, DateTime timestamp, Money money) : base(new ActivityId(0))
    {
        OwnerAccountId = ownerAccountId;
        SourceAccountId = sourceAccountId;
        TargetAccountId = targetAccountId;
        Timestamp = timestamp;
        Money = money;
    }
    
    public Activity(ActivityId? id, AccountId? ownerAccountId, AccountId? sourceAccountId, AccountId? targetAccountId, DateTime timestamp, Money money) : base(id)
    {
        OwnerAccountId = ownerAccountId;
        SourceAccountId = sourceAccountId;
        TargetAccountId = targetAccountId;
        Timestamp = timestamp;
        Money = money;
    }
}

public record ActivityId(long Id);
