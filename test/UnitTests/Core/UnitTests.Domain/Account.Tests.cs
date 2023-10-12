namespace Me.Acheddir.Hexagonal.UnitTests.Domain;

public class AccountTests
{
    [Fact]
    public void GivenAccountWithActivityWindow_WhenCalculatingBalance_ThenBalanceShouldBeCorrect()
    {
        // Arrange
        var accountId = new AccountId(1);
        var account = DefaultAccount()
            .WithAccountId(accountId)
            .WithBaselineBalance(Money.Of(555))
            .WithActivityWindow(new ActivityWindow(
                DefaultActivity()
                    .WithTargetAccount(accountId)
                    .WithMoney(Money.Of(999))
                    .Build(),
                DefaultActivity()
                    .WithTargetAccount(accountId)
                    .WithMoney(Money.Of(1)).Build()))
            .Build();

        // Act
        var balance = account.CalculateBalance();

        // Assert
        balance.Should().Be(Money.Of(1555));
    }

    [Fact]
    public void GivenAccountWithActivityWindow_WhenMakingWithdrawal_ThenBalanceShouldBeCorrect()
    {
        // Arrange
        var accountId = new AccountId(1L);
        var account = DefaultAccount()
            .WithAccountId(accountId)
            .WithBaselineBalance(Money.Of(555))
            .WithActivityWindow(new ActivityWindow(
                DefaultActivity()
                    .WithTargetAccount(accountId)
                    .WithMoney(Money.Of(999))
                    .Build(),
                DefaultActivity()
                    .WithTargetAccount(accountId)
                    .WithMoney(Money.Of(1)).Build()))
            .Build();

        // Act
        var targetAccountId = new AccountId(99);
        var success = account.Withdraw(Money.Of(555), targetAccountId);

        // Assert
        success.Should().BeTrue();
        account.ActivityWindow.GetActivities().Count.Should().Be(3);
        account.CalculateBalance().Should().Be(Money.Of(1000));
    }

    [Fact]
    public void GivenAccountWithActivityWindow_WhenMakingDeposit_ThenBalanceShouldBeCorrect()
    {
        // Arrange
        var accountId = new AccountId(1L);
        var account = DefaultAccount()
            .WithAccountId(accountId)
            .WithBaselineBalance(Money.Of(555))
            .WithActivityWindow(new ActivityWindow(
                DefaultActivity()
                    .WithTargetAccount(accountId)
                    .WithMoney(Money.Of(999))
                    .Build(),
                DefaultActivity()
                    .WithTargetAccount(accountId)
                    .WithMoney(Money.Of(1)).Build()))
            .Build();

        // Act
        var targetAccountId = new AccountId(99);
        var success = account.Deposit(Money.Of(445), targetAccountId);

        // Assert
        success.Should().BeTrue();
        account.ActivityWindow.GetActivities().Count.Should().Be(3);
        account.CalculateBalance().Should().Be(Money.Of(2000));
    }
}