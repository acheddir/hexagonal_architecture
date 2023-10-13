namespace Me.Acheddir.Hexagonal.UnitTests.Application;

public class GetAccountBalanceQueryTests
{
    [Fact]
    public async Task GivenAccountId_WhenGetAccountBalanceQueryIsIssued_ThenHandlerShouldReturnCorrectBalance()
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

        var accountLoadMock = new Mock<IAccountLoad>();
        accountLoadMock
            .Setup(
                al => al.LoadAccountAsync(
                    It.IsAny<AccountId>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var query = new GetAccountBalanceQuery(new AccountId(1), DateTime.UtcNow);
        var handler = new GetAccountBalanceQueryHandler(accountLoadMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().Be(Money.Of(1555));
    }
}