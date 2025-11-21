using BankingHelper.Belgium.Extensions;
using BankingHelper.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankingHelper.UnitTests.Belgium.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddBelgianBanking_RegistersPaymentReferenceGenerator()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddBelgianBanking();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service = serviceProvider.GetService<IPaymentReferenceGenerator>();
            service.Should().NotBeNull();
            service!.CountryCode.Should().Be("BE");
        }

        [Fact]
        public void AddBelgianBanking_RegistersServiceAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddBelgianBanking();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service1 = serviceProvider.GetService<IPaymentReferenceGenerator>();
            var service2 = serviceProvider.GetService<IPaymentReferenceGenerator>();
            
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().BeSameAs(service2);
        }

        [Fact]
        public void AddBelgianBanking_ReturnsServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddBelgianBanking();

            // Assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddBelgianBanking_AllowsMethodChaining()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert - Should not throw and allow chaining
            Action act = () => services
                .AddBelgianBanking()
                .AddBelgianBanking(); // Can be called multiple times

            act.Should().NotThrow();
        }

        [Fact]
        public void AddBelgianBanking_RegisteredService_CanGenerateReferences()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddBelgianBanking();
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IPaymentReferenceGenerator>();

            // Act
            var reference = service.Generate("12345");

            // Assert
            reference.Should().NotBeNullOrWhiteSpace();
            service.IsValid(reference).Should().BeTrue();
        }
    }
}
