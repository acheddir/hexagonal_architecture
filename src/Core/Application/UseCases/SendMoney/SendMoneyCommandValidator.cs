namespace Me.Acheddir.Hexagonal.Application.UseCases.SendMoney;

public class SendMoneyCommandValidator : AbstractValidator<SendMoneyCommand>
{
    public SendMoneyCommandValidator()
    {
        RuleFor(c => c.SourceAccountId).NotNull();
        RuleFor(c => c.TargetAccountId).NotNull();
        RuleFor(c => c.Amount).NotNull();
        RuleFor(c => c.Amount).Custom((money, context) =>
        {
            if (money is not null && !money.IsPositive())
                context.AddFailure("Money amount should be positive.");
        });
    } 
}
