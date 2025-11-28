using Finova.Netherlands.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Netherlands.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaNetherlands_RegistersIbanValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaNetherlands();
            var serviceProvider = services.BuildServiceProvider();

            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("NL");
        }

        [Fact]
        public void AddFinovaNetherlands_RegistersIbanParser()
        {
            var services = new ServiceCollection();
            services.AddFinovaNetherlands();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("NL");
        }

        [Fact]
        public void AddFinovaNetherlands_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddFinovaNetherlands();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaNetherlands_RegisteredValidator_CanValidateIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaNetherlands();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            validator.IsValidIban("NL91ABNA0417164300").Should().BeTrue();
        }

        [Fact]
        public void AddFinovaNetherlands_RegisteredParser_CanParseIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaNetherlands();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            var result = parser.ParseIban("NL91ABNA0417164300");
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("NL");
        }
    }
}
