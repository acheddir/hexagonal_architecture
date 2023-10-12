using Me.Acheddir.Hexagonal.Application.Ports.Driven;
using Me.Acheddir.Hexagonal.Application.UseCases.GetAccountBalance;
using Moq;

namespace Tests;

public class GetAccountBalanceQueryTests
{
    [Fact]
    public async void GivenAccountId_WhenGetAccountBalanceQueryIsIssued_ThenHandlerShouldReturnCorrectBalance()
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

        var accountLoad = new Mock<IAccountLoad>();
        accountLoad
            .Setup(
                al => al.LoadAccountAsync(
                    It.IsAny<AccountId>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(account));

        var query = new GetAccountBalanceQuery {AccountId = new AccountId(1)};
        var handler = new GetAccountBalanceQueryHandler(accountLoad.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value.Should().Be(Money.Of(1555));
    }
}