namespace Me.Acheddir.Hexagonal.Domain.Account;

public class ActivityWindow
{
    private readonly List<Activity> _activities;

    public ActivityWindow(List<Activity> activities)
    {
        _activities = activities;
    }

    public ActivityWindow(IEnumerable<Activity> activities)
    {
        _activities = activities.ToList();
    }

    public ActivityWindow(params Activity[] activities)
    {
        _activities = activities.ToList();
    }

    public DateTime GetStartTimestamp()
    {
        var startActivity = _activities.MinBy(a => a.Timestamp);

        if (startActivity is null)
            throw new NoActivityException("No activity was found.");

        return startActivity.Timestamp;
    }

    public DateTime GetEndTimestamp()
    {
        var endActivity = _activities.MaxBy(a => a.Timestamp);

        if (endActivity is null)
            throw new NoActivityException("No activity was found.");

        return endActivity.Timestamp;
    }

    public Money CalculateBalance(AccountId? accountId)
    {
        var depositBalance = _activities
            .Where(a => a.TargetAccountId.Equals(accountId))
            .Select(a => a.Money)
            .Aggregate(Money.Zero, Money.Add);

        var withdrawalBalance = _activities
            .Where(a => a.SourceAccountId.Equals(accountId))
            .Select(a => a.Money)
            .Aggregate(Money.Zero, Money.Add);
        
        return Money.Add(depositBalance, withdrawalBalance.Negate());
    }

    public ReadOnlyCollection<Activity> GetActivities()
    {
        return _activities.AsReadOnly();
    }

    public void AddActivity(Activity activity)
    {
        _activities.Add(activity);
    }
}
