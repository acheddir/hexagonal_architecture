namespace Me.Acheddir.Hexagonal.Application.UseCases.GetAccountBalance;

public record GetAccountBalanceQuery(AccountId? AccountId, DateTimeOffset BaselineDate) : IQuery<Money>;
