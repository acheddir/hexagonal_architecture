namespace Me.Acheddir.Hexagonal.Application.UseCases.GetAccountBalance;

public sealed class GetAccountBalanceQuery : IQuery<Money>
{
    public AccountId? AccountId { get; init; }
    
}
