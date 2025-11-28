using Finova.Spain.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Spain.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaSpain_RegistersIbanValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaSpain();
            var serviceProvider = services.BuildServiceProvider();

            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("ES");
        }

        [Fact]
        public void AddFinovaSpain_RegistersIbanParser()
        {
            var services = new ServiceCollection();
            services.AddFinovaSpain();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("ES");
        }

        [Fact]
        public void AddFinovaSpain_RegistersServicesAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddFinovaSpain();
            var serviceProvider = services.BuildServiceProvider();

            var validator1 = serviceProvider.GetService<IIbanValidator>();
            var validator2 = serviceProvider.GetService<IIbanValidator>();
            validator1.Should().BeSameAs(validator2);
        }

        [Fact]
        public void AddFinovaSpain_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddFinovaSpain();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaSpain_RegisteredValidator_CanValidateIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaSpain();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            var result = validator.IsValidIban("ES9121000418450200051332");
            result.Should().BeTrue();
        }

        [Fact]
        public void AddFinovaSpain_RegisteredParser_CanParseIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaSpain();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            var result = parser.ParseIban("ES9121000418450200051332");
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("ES");
        }

        [Fact]
        public void AddFinovaSpain_ParserDependsOnValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaSpain();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            var validator = serviceProvider.GetService<IIbanValidator>();

            parser.Should().NotBeNull();
            validator.Should().NotBeNull();
        }

        [Fact]
        public void AddFinovaSpain_CanBeCalledMultipleTimes()
        {
            var services = new ServiceCollection();
            services.AddFinovaSpain();
            services.AddFinovaSpain(); // Should not throw
            var serviceProvider = services.BuildServiceProvider();

            var validators = serviceProvider.GetServices<IIbanValidator>();
            validators.Should().NotBeEmpty();
        }
    }
}
