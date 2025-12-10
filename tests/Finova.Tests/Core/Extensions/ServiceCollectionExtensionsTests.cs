using Finova.Core.Extensions;
using Finova.Core.Iban;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Core.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddFinovaCoreServices_RegistersIIbanService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFinovaCoreServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var service = serviceProvider.GetService<IIbanService>();
        service.Should().NotBeNull();
        service.Should().BeOfType<IbanService>();
    }

    [Fact]
    public void AddFinovaCoreServices_RegistersServiceAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFinovaCoreServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var service1 = serviceProvider.GetService<IIbanService>();
        var service2 = serviceProvider.GetService<IIbanService>();

        service1.Should().NotBeNull();
        service2.Should().NotBeNull();
        service1.Should().BeSameAs(service2);
    }

    [Fact]
    public void AddFinovaCoreServices_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddFinovaCoreServices();

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddFinovaCoreServices_AllowsMethodChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        Action act = () => services
            .AddFinovaCoreServices()
            .AddFinovaCoreServices();

        act.Should().NotThrow();
    }

    [Fact]
    public void AddFinovaCoreServices_RegisteredService_CanValidateIbans()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFinovaCoreServices();
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<IIbanService>();

        // Act
        var result = service.Validate("BE68539007547034");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void AddFinovaCoreServices_RegisteredService_CanFormatIbans()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFinovaCoreServices();
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<IIbanService>();

        // Act
        var result = service.FormatIban("BE68539007547034");

        // Assert
        result.Should().Be("BE68 5390 0754 7034");
    }
}
