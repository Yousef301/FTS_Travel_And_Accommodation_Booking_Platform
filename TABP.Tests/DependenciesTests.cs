using System.Reflection;
using FluentAssertions;

namespace TABP.Tests;

public class DependenciesTests
{
    private const string ApiNamespace = "TABP.Api";
    private const string ApplicationNamespace = "TABP.Application";
    private const string DaNamespace = "TABP.DA";

    [Fact]
    public void ApiNamespace_ShouldNotReference_DANamespace()
    {
        // Arrange
        var apiAssembly = Assembly.Load(ApiNamespace);
        var daAssembly = Assembly.Load(DaNamespace);

        // Act
        var apiTypes = apiAssembly.GetTypes();
        var daTypes = daAssembly.GetTypes();

        // Assert
        foreach (var type in apiTypes)
        {
            var dependencies = type.GetInterfaces().Concat(type.GetNestedTypes());
            dependencies.Should().NotContain(dependency => daTypes.Contains(dependency));
        }
    }

    [Fact]
    public void ApplicationNamespace_ShouldNotReference_ApiNamespace()
    {
        // Arrange
        var applicationAssembly = Assembly.Load(ApplicationNamespace);
        var apiAssembly = Assembly.Load(ApiNamespace);

        // Act
        var applicationTypes = applicationAssembly.GetTypes();
        var apiTypes = apiAssembly.GetTypes();

        // Assert
        foreach (var type in applicationTypes)
        {
            var dependencies = type.GetInterfaces().Concat(type.GetNestedTypes());
            dependencies.Should().NotContain(dependency => apiTypes.Contains(dependency));
        }
    }

    [Fact]
    public void DANamespace_ShouldNotReference_OtherNamespaces()
    {
        // Arrange
        var daAssembly = Assembly.Load(DaNamespace);
        var apiAssembly = Assembly.Load(ApiNamespace);
        var applicationAssembly = Assembly.Load(ApplicationNamespace);

        // Act
        var daTypes = daAssembly.GetTypes();
        var apiTypes = apiAssembly.GetTypes();
        var applicationTypes = applicationAssembly.GetTypes();

        // Assert
        foreach (var type in daTypes)
        {
            var dependencies = type.GetInterfaces().Concat(type.GetNestedTypes());
            dependencies.Should().NotContain(dependency =>
                apiTypes.Contains(dependency) ||
                applicationTypes.Contains(dependency));
        }
    }
}