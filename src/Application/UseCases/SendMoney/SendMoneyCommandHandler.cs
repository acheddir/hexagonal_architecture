namespace Me.Acheddir.Hexagonal.Application.UseCases.SendMoney;

public sealed class SendMoneyCommandHandler : ICommandHandler<SendMoneyCommand>
{
    private readonly IAccountLoad _accountLoad;
    private readonly IAccountLock _accountLock;
    private readonly IAccountUpdate _accountUpdate;
    private readonly MoneyTransferOptions _moneyTransferOptions;
    
    public SendMoneyCommandHandler(
        IAccountLoad accountLoad,
        IAccountLock accountLock,
        IAccountUpdate accountUpdate,
        IOptions<MoneyTransferOptions> moneyTransferOptions)
    {
        _accountLoad = accountLoad;
        _accountLock = accountLock;
        _moneyTransferOptions = moneyTransferOptions.Value;
        _accountUpdate = accountUpdate;
    }
    
    public async Task<Result> Handle(SendMoneyCommand command, CancellationToken cancellationToken)
    {
        CheckThreshold(command);
        
        var baselineDate = UtcNow.AddDays(-10);

        var sourceAccount = await _accountLoad.LoadAccountAsync(command.SourceAccountId, baselineDate, cancellationToken);
        var targetAccount = await _accountLoad.LoadAccountAsync(command.TargetAccountId, baselineDate, cancellationToken);

        var sourceAccountId = sourceAccount.GetId()
                              ?? throw new IllegalAccountStateException("Expected Non-Null Source Account ID");
        var targetAccountId = targetAccount.GetId()
                              ?? throw new IllegalAccountStateException("Expected Non-Null Target Account ID");
        
        await _accountLock.LockAccountAsync(sourceAccountId, cancellationToken);

        var withdrawalIsSuccessful = sourceAccount.Withdraw(command.Amount, targetAccountId);
        if (!withdrawalIsSuccessful)
        {
            await _accountLock.ReleaseAccountAsync(sourceAccountId, cancellationToken);
            return Result.Fail("Unsuccessful withdrawal");
        }

        await _accountLock.LockAccountAsync(targetAccountId, cancellationToken);
        var depositIsSuccessful = targetAccount.Deposit(command.Amount, sourceAccountId);
        
        if (!depositIsSuccessful)
        {
            await _accountLock.ReleaseAccountAsync(sourceAccountId, cancellationToken);
            await _accountLock.ReleaseAccountAsync(targetAccountId, cancellationToken);
            return Result.Fail("Unsuccessful deposit");
        }

        await _accountUpdate.UpdateActivitiesAsync(sourceAccountId, cancellationToken);
        await _accountUpdate.UpdateActivitiesAsync(targetAccountId, cancellationToken);

        await _accountLock.ReleaseAccountAsync(sourceAccountId, cancellationToken);
        await _accountLock.ReleaseAccountAsync(targetAccountId, cancellationToken);

        return Result.Ok();
    }
    
    private void CheckThreshold(SendMoneyCommand command)
    {
        if (command.Amount.IsGreaterThan(_moneyTransferOptions.MaximumTransferThreshold))
            throw new ThresholdExceededException(
                _moneyTransferOptions.MaximumTransferThreshold,
                command.Amount);
    }
}
