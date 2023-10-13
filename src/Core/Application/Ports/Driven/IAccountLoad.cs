using Me.Acheddir.Hexagonal.SharedKernel.Ports.Driven;

namespace Me.Acheddir.Hexagonal.Application.Ports.Driven;

public interface IAccountLoad : IDrivenPort
{
    Task<Account> LoadAccountAsync(AccountId accountId, DateTime baselineDate, CancellationToken token);
}
