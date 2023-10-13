namespace Me.Acheddir.Hexagonal.SharedKernel.Domain;

public interface IUnitOfWork
{
    Task<int> Commit();
}
