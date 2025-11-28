using Finova.Belgium.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Belgium.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaBelgium_RegistersPaymentReferenceGenerator()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddFinovaBelgium();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service = serviceProvider.GetService<IPaymentReferenceGenerator>();
            service.Should().NotBeNull();
            service!.CountryCode.Should().Be("BE");
        }

        [Fact]
        public void AddFinovaBelgium_RegistersIbanValidator()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddFinovaBelgium();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("BE");
        }

        [Fact]
        public void AddFinovaBelgium_RegistersIbanParser()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddFinovaBelgium();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("BE");
        }

        [Fact]
        public void AddFinovaBelgium_RegistersServicesAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddFinovaBelgium();
            var serviceProvider = services.BuildServiceProvider();

            // Assert - Payment Reference Generator
            var service1 = serviceProvider.GetService<IPaymentReferenceGenerator>();
            var service2 = serviceProvider.GetService<IPaymentReferenceGenerator>();
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().BeSameAs(service2);

            // Assert - IBAN Validator
            var validator1 = serviceProvider.GetService<IIbanValidator>();
            var validator2 = serviceProvider.GetService<IIbanValidator>();
            validator1.Should().NotBeNull();
            validator2.Should().NotBeNull();
            validator1.Should().BeSameAs(validator2);

            // Assert - IBAN Parser
            var parser1 = serviceProvider.GetService<IIbanParser>();
            var parser2 = serviceProvider.GetService<IIbanParser>();
            parser1.Should().NotBeNull();
            parser2.Should().NotBeNull();
            parser1.Should().BeSameAs(parser2);
        }

        [Fact]
        public void AddFinovaBelgium_ReturnsServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddFinovaBelgium();

            // Assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaBelgium_AllowsMethodChaining()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert - Should not throw and allow chaining
            Action act = () => services
                .AddFinovaBelgium()
                .AddFinovaBelgium(); // Can be called multiple times

            act.Should().NotThrow();
        }

        [Fact]
        public void AddFinovaBelgium_RegisteredPaymentService_CanGenerateReferences()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFinovaBelgium();
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IPaymentReferenceGenerator>();

            // Act
            var reference = service.Generate("12345");

            // Assert
            reference.Should().NotBeNullOrWhiteSpace();
            service.IsValid(reference).Should().BeTrue();
        }

        [Fact]
        public void AddFinovaBelgium_RegisteredValidator_CanValidateIbans()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFinovaBelgium();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            // Act
            var result = validator.IsValidIban("BE68539007547034");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AddFinovaBelgium_RegisteredParser_CanParseIbans()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddFinovaBelgium();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            // Act
            var result = parser.ParseIban("BE68539007547034");

            // Assert
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("BE");
            result.IsValid.Should().BeTrue();
        }
    }
}
