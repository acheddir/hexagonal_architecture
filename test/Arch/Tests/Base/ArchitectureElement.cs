using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Me.Acheddir.Hexagonal.Arch.Tests.Base;

public abstract class ArchitectureElement
{
    private readonly IObjectProvider<IType> _assembly;
    private readonly Architecture _architecture;

    protected ArchitectureElement(IObjectProvider<IType> assembly, Architecture architecture)
    {
        _assembly = assembly;
        _architecture = architecture;
    }

    public void DenyDependency(
        IObjectProvider<IType> dependency)
    {
        Types()
            .That()
            .Are(_assembly)
            .Should()
            .NotDependOnAny(dependency)
            .Check(_architecture);
    }

    public void DenyAnyDependency(IEnumerable<IObjectProvider<IType>> dependencies)
    {
        foreach (var dependency in dependencies)
        {
            DenyDependency(dependency);
        }
    }

    public void DenyEmptyNamespace(string namespacePattern)
    {
        Types()
            .That()
            .Are(_assembly)
            .And()
            .ResideInNamespace(namespacePattern, true)
            .Should()
            .Exist()
            .Check(_architecture);
    }

    public void DenyEmptyNamespaces(IEnumerable<string> namespaces)
    {
        foreach (var @namespace in namespaces)
        {
            DenyEmptyNamespace(@namespace);
        }
    }
}