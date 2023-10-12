using Me.Acheddir.Hexagonal.Application.Exceptions;
using Me.Acheddir.Hexagonal.Application.UseCases.SendMoney;
using Microsoft.Extensions.Options;

namespace Me.Acheddir.Hexagonal.UnitTests.Application;

public class SendMoneyCommandTests
{
    private readonly AccountId _sourceAccountId = new AccountId(1);
    private readonly AccountId _targetAccountId = new AccountId(2);

    private readonly Mock<Account> _sourceAccount = new();
    private readonly Mock<Account> _targetAccount = new();
    private readonly Mock<IAccountLoad> _accountLoadMock = new();
    private readonly Mock<IAccountLock> _accountLockMock = new();
    private readonly Mock<IAccountUpdate> _accountUpdateMock = new();

    private readonly IOptions<MoneyTransferOptions> _moneyTransferOptions =
        Options.Create(new MoneyTransferOptions {MaximumTransferThreshold = Money.Of(1000)});

    [Fact]
    public void GivenNullSourceAccountId_WhenSendMoneyCommandIsIssued_ThenShouldThrowValidationException()
    {
        // Arrange
        var command = new SendMoneyCommand
        {
            SourceAccountId = null
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        // Act & Assert
        handler.Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullTargetAccountId_WhenSendMoneyCommandIsIssued_ThenShouldThrowValidationException()
    {
        // Arrange
        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = null
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        // Act & Assert
        handler.Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullAmount_WhenSendMoneyCommandIsIssued_ThenShouldThrowValidationException()
    {
        // Arrange
        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = _targetAccountId,
            Amount = null
        };
        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        // Act & Assert
        handler.Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public void
        GivenAmountIsGreaterThanMaximumThreshold_WhenSendMoneyCommandIsIssued_ThenShouldThrowThresholdExceededException()
    {
        // Arrange
        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = _targetAccountId,
            Amount = Money.Of(1200)
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        // Act & Assert
        handler.Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ThresholdExceededException>()
            .WithMessage(
                "Maximum threshold for transferring money exceeded: tried to transfer 1500 but threshold is 1000");
    }

    [Fact]
    public void GivenSendMoneyCommandIsIssued_ThenThrowIllegalAccountStateException_IfSourceAccountIsNull()
    {
        // Arrange
        SetupSourceAccount(null, _sourceAccountId);

        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = _targetAccountId,
            Amount = Money.Of(100)
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        // Act && Assert
        handler.Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<IllegalAccountStateException>()
            .WithMessage("Expected non-null source account ID");
    }

    [Fact]
    public void GivenSendMoneyCommandIsIssued_ThenThrowIllegalAccountStateException_IfTargetAccountIsNull()
    {
        // Arrange
        SetupSourceAccount(_sourceAccountId, _sourceAccountId);
        SetupTargetAccount(null, _targetAccountId);

        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = _targetAccountId,
            Amount = Money.Of(100)
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        // Act & Assert
        handler.Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<IllegalAccountStateException>()
            .WithMessage("Expected non-null target account ID");
    }

    [Fact]
    public async Task GivenWithdrawalFails_ThenOnlySourceAccountIsLockedAndReleased()
    {
        var amount = Money.Of(100);

        SetupSourceAccount(_sourceAccountId, _sourceAccountId);
        SetupTargetAccount(_targetAccountId, _targetAccountId);

        _sourceAccount.Setup(a => a.Withdraw(amount, _targetAccountId)).Returns(false);

        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = _targetAccountId,
            Amount = amount
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Unsuccessful withdrawal");
        _accountLockMock.Verify(al => al.LockAccountAsync(_sourceAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.ReleaseAccountAsync(_sourceAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.LockAccountAsync(_targetAccountId, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task GivenDepositFails_ThenBothSourceAndTargetAccountsAreLockedAndReleased()
    {
        var amount = Money.Of(100);

        SetupSourceAccount(_sourceAccountId, _sourceAccountId);
        SetupTargetAccount(_targetAccountId, _targetAccountId);

        _sourceAccount.Setup(a => a.Withdraw(amount, _targetAccountId)).Returns(true);
        _targetAccount.Setup(a => a.Deposit(amount, _sourceAccountId)).Returns(false);

        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = _targetAccountId,
            Amount = amount
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Unsuccessful deposit");
        _accountLockMock.Verify(al => al.LockAccountAsync(_sourceAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.ReleaseAccountAsync(_sourceAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.LockAccountAsync(_targetAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.ReleaseAccountAsync(_targetAccountId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task
        GivenTransactionSucceeds_ThenBothSourceAndTargetAccountsAreLockedAndReleased_AndActivitiesAreUpdated_AndResultIsOk()
    {
        var updatedAccountIds = new List<AccountId>();
        
        var amount = Money.Of(100);

        SetupSourceAccount(_sourceAccountId, _sourceAccountId);
        SetupTargetAccount(_targetAccountId, _targetAccountId);

        _sourceAccount.Setup(a => a.Withdraw(amount, _targetAccountId)).Returns(true);
        _targetAccount.Setup(a => a.Deposit(amount, _sourceAccountId)).Returns(true);

        _accountUpdateMock.Setup(au => au.UpdateActivitiesAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Callback<Account, CancellationToken>((account, token) =>
            {
                updatedAccountIds.Add(account.GetId()!);
            });

        var command = new SendMoneyCommand
        {
            SourceAccountId = _sourceAccountId,
            TargetAccountId = _targetAccountId,
            Amount = amount
        };

        var handler = new SendMoneyCommandHandler(
            _accountLoadMock.Object,
            _accountLockMock.Object,
            _accountUpdateMock.Object,
            _moneyTransferOptions);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _accountLockMock.Verify(al => al.LockAccountAsync(_sourceAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.ReleaseAccountAsync(_sourceAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.LockAccountAsync(_targetAccountId, CancellationToken.None), Times.Once);
        _accountLockMock.Verify(al => al.ReleaseAccountAsync(_targetAccountId, CancellationToken.None), Times.Once);
        
        _accountUpdateMock.Verify(au => au.UpdateActivitiesAsync(It.IsAny<Account>(), CancellationToken.None), Times.Exactly(2));
        updatedAccountIds.Should().Contain(new [] { _sourceAccountId, _targetAccountId });
    }

    private void SetupSourceAccount(AccountId? getIdResult, AccountId accountIdArgument)
    {
        _sourceAccount.Setup(a => a.GetId()).Returns(getIdResult);
        _accountLoadMock
            .Setup(al => al.LoadAccountAsync(accountIdArgument, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sourceAccount.Object);
    }

    private void SetupTargetAccount(AccountId? getIdResult, AccountId accountIdArgument)
    {
        _targetAccount.Setup(a => a.GetId()).Returns(getIdResult);
        _accountLoadMock
            .Setup(al => al.LoadAccountAsync(accountIdArgument, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_targetAccount.Object);
    }
}