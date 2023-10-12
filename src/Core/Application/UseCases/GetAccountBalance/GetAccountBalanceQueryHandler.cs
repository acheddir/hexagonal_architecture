namespace Me.Acheddir.Hexagonal.Application.UseCases.GetAccountBalance;

public sealed class GetAccountBalanceQueryHandler : IQueryHandler<GetAccountBalanceQuery, Money>
{
    private readonly IAccountLoad _accountLoad;

    public GetAccountBalanceQueryHandler(IAccountLoad accountLoad)
    {
        _accountLoad = accountLoad;
    }
    
    public async Task<Result<Money>> Handle(GetAccountBalanceQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query.AccountId, nameof(query.AccountId));
        
        var account = await _accountLoad
            .LoadAccountAsync(query.AccountId, UtcNow, cancellationToken);

        var balance = account.CalculateBalance();

        return Result.Ok(balance);
    }
}