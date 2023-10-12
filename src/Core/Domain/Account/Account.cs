namespace Me.Acheddir.Hexagonal.Domain.Account;

public class Account : Entity<AccountId>, IAggregateRoot
{
    private Money BaselineBalance { get; }
    public ActivityWindow ActivityWindow { get; }

    // Used by Moq
    protected Account() : base(null)
    {
        BaselineBalance = Money.Of(0);
        ActivityWindow = new ActivityWindow(Array.Empty<Activity>());
    }

    public static Account CreateInstance()
    {
        return new Account();
    }

    private Account(AccountId id, Money baselineBalance, ActivityWindow activityWindow) : base(id)
    {
        BaselineBalance = baselineBalance;
        ActivityWindow = activityWindow;
    }

    public static Account WithoutId(Money baselineBalance, ActivityWindow activityWindow)
    {
        return new Account(new AccountId(0), baselineBalance, activityWindow);
    }

    public static Account WithId(AccountId id, Money baselineBalance, ActivityWindow activityWindow)
    {
        return new Account(id, baselineBalance, activityWindow);
    }

    public virtual AccountId? GetId()
    {
        return Id;
    }

    public Money CalculateBalance()
    {
        return Money.Add(
            BaselineBalance,
            ActivityWindow.CalculateBalance(Id));
    }

    public virtual bool Withdraw(Money money, AccountId targetAccountId)
    {
        if (!MayWithdraw(money))
            return false;

        var withdrawal = new Activity(
            Id,
            Id,
            targetAccountId,
            DateTime.UtcNow,
            money
        );
        
        ActivityWindow.AddActivity(withdrawal);
        
        return true;
    }

    private bool MayWithdraw(Money money)
    {
        return Money
            .Add(
                CalculateBalance(),
                money.Negate())
            .IsPositiveOrZero();
    }

    public virtual bool Deposit(Money money, AccountId sourceAccountId)
    {
        var deposit = new Activity(
            Id,
            sourceAccountId,
            Id,
            DateTime.UtcNow,
            money
        );
        
        ActivityWindow.AddActivity(deposit);

        return true;
    }
}

public record AccountId(long Id);
