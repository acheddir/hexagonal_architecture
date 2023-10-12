namespace Me.Acheddir.Hexagonal.Application.Ports.Driven;

public interface IAccountUpdate
{
    Task UpdateActivitiesAsync(Account account, CancellationToken token);
}
