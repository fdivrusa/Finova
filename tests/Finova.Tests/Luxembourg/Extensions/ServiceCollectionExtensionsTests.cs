using Finova.Luxembourg.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Luxembourg.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaLuxembourg_RegistersIbanValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaLuxembourg();
            var serviceProvider = services.BuildServiceProvider();

            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("LU");
        }

        [Fact]
        public void AddFinovaLuxembourg_RegistersIbanParser()
        {
            var services = new ServiceCollection();
            services.AddFinovaLuxembourg();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("LU");
        }

        [Fact]
        public void AddFinovaLuxembourg_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddFinovaLuxembourg();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaLuxembourg_RegisteredValidator_CanValidateIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaLuxembourg();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            validator.IsValidIban("LU280019400644750000").Should().BeTrue();
        }

        [Fact]
        public void AddFinovaLuxembourg_RegisteredParser_CanParseIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaLuxembourg();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            var result = parser.ParseIban("LU280019400644750000");
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("LU");
        }
    }
}
