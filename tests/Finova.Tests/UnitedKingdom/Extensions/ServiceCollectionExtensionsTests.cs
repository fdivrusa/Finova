using Finova.UnitedKingdom.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.UnitedKingdom.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaUnitedKingdom_RegistersIbanValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaUnitedKingdom();
            var serviceProvider = services.BuildServiceProvider();

            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("GB");
        }

        [Fact]
        public void AddFinovaUnitedKingdom_RegistersIbanParser()
        {
            var services = new ServiceCollection();
            services.AddFinovaUnitedKingdom();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("GB");
        }

        [Fact]
        public void AddFinovaUnitedKingdom_RegistersServicesAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddFinovaUnitedKingdom();
            var serviceProvider = services.BuildServiceProvider();

            var validator1 = serviceProvider.GetService<IIbanValidator>();
            var validator2 = serviceProvider.GetService<IIbanValidator>();
            validator1.Should().BeSameAs(validator2);
        }

        [Fact]
        public void AddFinovaUnitedKingdom_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddFinovaUnitedKingdom();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaUnitedKingdom_RegisteredValidator_CanValidateIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaUnitedKingdom();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            validator.IsValidIban("GB29NWBK60161331926819").Should().BeTrue();
        }

        [Fact]
        public void AddFinovaUnitedKingdom_RegisteredParser_CanParseIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaUnitedKingdom();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            var result = parser.ParseIban("GB29NWBK60161331926819");
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("GB");
        }

        [Fact]
        public void AddFinovaUnitedKingdom_AllowsMethodChaining()
        {
            var services = new ServiceCollection();
            Action act = () => services
                .AddFinovaUnitedKingdom()
                .AddFinovaUnitedKingdom();

            act.Should().NotThrow();
        }
    }
}
