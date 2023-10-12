using Me.Acheddir.Hexagonal.Domain.Account;

namespace Tests.Core;

public class ActivityTestData
{
    public static ActivityBuilder DefaultActivity()
    {
        return new ActivityBuilder()
            .WithOwnerAccount(new AccountId(42))
            .WithSourceAccount(new AccountId(42))
            .WithTargetAccount(new AccountId(41))
            .WithTimestamp(DateTime.Now)
            .WithMoney(Money.Of(999));
    }
    
    public class ActivityBuilder
    {
        private ActivityId _activityId;
        private AccountId _ownerAccountId;
        private AccountId _sourceAccountId;
        private AccountId _targetAccountId;
        private DateTime _timestamp;
        private Money _money;

        public ActivityBuilder WithId(ActivityId activityId)
        {
            _activityId = activityId;
            return this;
        }

        public ActivityBuilder WithOwnerAccount(AccountId ownerAccountId)
        {
            _ownerAccountId = ownerAccountId;
            return this;
        }

        public ActivityBuilder WithSourceAccount(AccountId sourceAccountId)
        {
            _sourceAccountId = sourceAccountId;
            return this;
        }

        public ActivityBuilder WithTargetAccount(AccountId targetAccountId)
        {
            _targetAccountId = targetAccountId;
            return this;
        }

        public ActivityBuilder WithTimestamp(DateTime timestamp)
        {
            _timestamp = timestamp;
            return this;
        }

        public ActivityBuilder WithMoney(Money money)
        {
            _money = money;
            return this;
        }

        public Activity Build()
        {
            return new Activity(
                _activityId,
                _ownerAccountId,
                _sourceAccountId,
                _targetAccountId,
                _timestamp,
                _money);
        }
    }
}