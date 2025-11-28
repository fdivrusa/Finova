using Finova.Germany.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Germany.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaGermany_RegistersIbanValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaGermany();
            var serviceProvider = services.BuildServiceProvider();

            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("DE");
        }

        [Fact]
        public void AddFinovaGermany_RegistersIbanParser()
        {
            var services = new ServiceCollection();
            services.AddFinovaGermany();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("DE");
        }

        [Fact]
        public void AddFinovaGermany_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddFinovaGermany();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaGermany_RegisteredValidator_CanValidateIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaGermany();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            validator.IsValidIban("DE89370400440532013000").Should().BeTrue();
        }

        [Fact]
        public void AddFinovaGermany_RegisteredParser_CanParseIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaGermany();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            var result = parser.ParseIban("DE89370400440532013000");
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("DE");
        }
    }
}
