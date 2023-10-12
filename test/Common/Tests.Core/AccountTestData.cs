using Me.Acheddir.Hexagonal.Domain.Account;

namespace Tests.Core;

public class AccountTestData
{
    public static AccountBuilder DefaultAccount()
    {
        return new AccountBuilder()
            .WithAccountId(new AccountId(42))
            .WithBaselineBalance(Money.Of(999))
            .WithActivityWindow(new ActivityWindow(
                ActivityTestData.DefaultActivity().WithId(new ActivityId(1)).Build(),
                ActivityTestData.DefaultActivity().WithId(new ActivityId(2)).Build()));
    }

    public class AccountBuilder
    {
        private AccountId _accountId;
        private Money _baselineBalance;
        private ActivityWindow _activityWindow;

        public AccountBuilder WithAccountId(AccountId accountId)
        {
            _accountId = accountId;
            return this;
        }

        public AccountBuilder WithBaselineBalance(Money baselineBalance)
        {
            _baselineBalance = baselineBalance;
            return this;
        }

        public AccountBuilder WithActivityWindow(ActivityWindow activityWindow)
        {
            _activityWindow = activityWindow;
            return this;
        }

        public Account Build()
        {
            return Account.WithId(
                _accountId,
                _baselineBalance,
                _activityWindow);
        }
    }
}