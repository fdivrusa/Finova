using Finova.France.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.France.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaFrance_RegistersIbanValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaFrance();
            var serviceProvider = services.BuildServiceProvider();

            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("FR");
        }

        [Fact]
        public void AddFinovaFrance_RegistersIbanParser()
        {
            var services = new ServiceCollection();
            services.AddFinovaFrance();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("FR");
        }

        [Fact]
        public void AddFinovaFrance_RegistersServicesAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddFinovaFrance();
            var serviceProvider = services.BuildServiceProvider();

            var validator1 = serviceProvider.GetService<IIbanValidator>();
            var validator2 = serviceProvider.GetService<IIbanValidator>();
            validator1.Should().BeSameAs(validator2);
        }

        [Fact]
        public void AddFinovaFrance_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddFinovaFrance();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaFrance_RegisteredValidator_CanValidateIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaFrance();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            var result = validator.IsValidIban("FR1420041010050500013M02606");
            result.Should().BeTrue();
        }

        [Fact]
        public void AddFinovaFrance_RegisteredParser_CanParseIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaFrance();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            var result = parser.ParseIban("FR1420041010050500013M02606");
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("FR");
        }
    }
}
