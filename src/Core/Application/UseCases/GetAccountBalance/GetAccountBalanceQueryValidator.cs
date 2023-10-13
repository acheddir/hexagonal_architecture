namespace Me.Acheddir.Hexagonal.Application.UseCases.GetAccountBalance;

public class GetAccountBalanceQueryValidator : AbstractValidator<GetAccountBalanceQuery>
{
    public GetAccountBalanceQueryValidator()
    {
        RuleFor(q => q.AccountId).NotNull();
        RuleFor(q => q.BaselineDate).Custom((date, context) =>
        {
            if (date.UtcDateTime >= UtcNow)
                context.AddFailure("Baseline date must be earlier than now.");
        });
    }
}