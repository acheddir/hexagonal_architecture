﻿namespace Me.Acheddir.Hexagonal.Persistence.Account.Mapping;

public static class MappingExtensions
{
    public static Domain.Account.Account MapToDomainAccount(
        this (AccountEntity Account, List<ActivityEntity> Activities, long WitdhdrawalBalance, long DepositBalance)
            sources)
    {
        var baselineBalance =
            Money.Subtract(Money.Of(sources.DepositBalance), Money.Of(sources.WitdhdrawalBalance));

        var activities = sources.Activities.Select(a => new Activity(
            new ActivityId(a.Id),
            new AccountId(a.OwnerAccountId),
            new AccountId(a.SourceAccountId),
            new AccountId(a.TargetAccountId),
            a.Timestamp,
            Money.Of(a.Amount)
        ));

        var activityWindow = new ActivityWindow(activities);

        return Domain.Account.Account.WithId(new AccountId(sources.Account.Id), baselineBalance, activityWindow);
    }
}