namespace Me.Acheddir.Hexagonal.Application.Ports.Driven;

public interface IAccountLoad
{
    Task<Account> LoadAccountAsync(AccountId requestAccountId, DateTime utcNow, CancellationToken token);
}
