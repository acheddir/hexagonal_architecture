namespace Me.Acheddir.Hexagonal.Application.Ports.Driven;

public interface IAccountUpdate
{
    Task UpdateActivitiesAsync(AccountId accountId, CancellationToken token);
}
