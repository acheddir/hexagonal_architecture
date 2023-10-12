namespace Me.Acheddir.Hexagonal.Arch.Tests.Base;

public class DomainElement : ArchitectureElement
{
    public DomainElement(IObjectProvider<IType> assembly, Architecture architecture)
        : base(assembly, architecture)
    {
    }
}