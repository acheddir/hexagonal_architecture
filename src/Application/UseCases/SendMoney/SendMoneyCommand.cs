namespace Me.Acheddir.Hexagonal.Application.UseCases.SendMoney;

public sealed class SendMoneyCommand : ICommand
{
    public AccountId SourceAccountId { get; set; }
    public AccountId TargetAccountId { get; set; }
    public Money Amount { get; set; }
}
