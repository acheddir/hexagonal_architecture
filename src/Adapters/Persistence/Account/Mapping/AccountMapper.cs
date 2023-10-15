namespace Me.Acheddir.Hexagonal.Persistence.Account.Mapping;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<(AccountEntity Account, List<ActivityEntity> Activities, long WitdhdrawalBalance, long DepositBalance)
                , Domain.Account.Account>()
            .ConstructUsing(s => s.MapToDomainAccount());
    }
}