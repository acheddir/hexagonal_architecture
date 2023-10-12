namespace Me.Acheddir.Hexagonal.Architecture.Tests.Base;

public class DomainElement : ArchitectureElement
{
    public DomainElement(IObjectProvider<IType> assembly, ArchUnitNET.Domain.Architecture architecture)
        : base(assembly, architecture)
    {
    }
}