namespace Me.Acheddir.Hexagonal.Application.UseCases.SendMoney;

public record SendMoneyCommand(AccountId? SourceAccountId, AccountId? TargetAccountId, Money? Amount) : ICommand;
