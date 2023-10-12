using System.Net.Mime;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Me.Acheddir.Hexagonal.Architecture.Tests;

public class DependencyTests
{
    private static readonly ArchUnitNET.Domain.Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            SharedKernel.AssemblyReference.Assembly,
            Domain.AssemblyReference.Assembly,
            Application.AssemblyReference.Assembly)
        .Build();
    
    private readonly IObjectProvider<IType> DomainAssembly =
        Types()
            .That()
            .ResideInAssembly(Domain.AssemblyReference.Assembly)
            .As("Domain Types");
    
    private readonly IObjectProvider<IType> ApplicationAssembly =
        Types()
            .That()
            .ResideInAssembly(Application.AssemblyReference.Assembly)
            .As("Application Types");
    
    [Fact]
    public void DomainAssembly_ShouldNot_HaveAnyDependencyOnApplicationAssembly()
    {
        Types()
            .That()
            .Are(DomainAssembly)
            .Should()
            .NotDependOnAny(ApplicationAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void DomainTypes_Should_Exist()
    {
        Types()
            .That()
            .Are(DomainAssembly)
            .And()
            .ResideInNamespace(@"Me.Acheddir.Hexagonal.Domain.\w+", true)
            .Should()
            .Exist()
            .Check(Architecture);
    }
}