namespace Me.Acheddir.Hexagonal.Application.UseCases.SendMoney;

public sealed class MoneyTransferOptions
{
    public Money MaximumTransferThreshold { get; set; } = Money.Of(1_000_000);
}
