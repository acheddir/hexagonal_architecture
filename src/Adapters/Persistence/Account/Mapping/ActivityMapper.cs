namespace Me.Acheddir.Hexagonal.Persistence.Account.Mapping;

public class ActivityMapper : Profile
{
    public ActivityMapper()
    {
        CreateMap<Activity, ActivityEntity>()
            .ForMember(d => d.Id, options => options.Ignore())
            .ForMember(d => d.OwnerAccountId, options => options.MapFrom(s => s.OwnerAccountId!.Id))
            .ForMember(d => d.SourceAccountId, options => options.MapFrom(s => s.SourceAccountId!.Id))
            .ForMember(d => d.TargetAccountId, options => options.MapFrom(s => s.TargetAccountId!.Id))
            .ForMember(d => d.Amount, options => options.MapFrom(s => s.Money.Amount));
    }
}
