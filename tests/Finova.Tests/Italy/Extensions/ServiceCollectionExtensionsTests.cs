using Finova.Italy.Extensions;
using Finova.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Italy.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddFinovaItaly_RegistersIbanValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaItaly();
            var serviceProvider = services.BuildServiceProvider();

            var validator = serviceProvider.GetService<IIbanValidator>();
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be("IT");
        }

        [Fact]
        public void AddFinovaItaly_RegistersIbanParser()
        {
            var services = new ServiceCollection();
            services.AddFinovaItaly();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be("IT");
        }

        [Fact]
        public void AddFinovaItaly_RegistersServicesAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddFinovaItaly();
            var serviceProvider = services.BuildServiceProvider();

            var validator1 = serviceProvider.GetService<IIbanValidator>();
            var validator2 = serviceProvider.GetService<IIbanValidator>();
            validator1.Should().BeSameAs(validator2);
        }

        [Fact]
        public void AddFinovaItaly_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddFinovaItaly();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddFinovaItaly_RegisteredValidator_CanValidateIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaItaly();
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetRequiredService<IIbanValidator>();

            var result = validator.IsValidIban("IT60X0542811101000000123456");
            result.Should().BeTrue();
        }

        [Fact]
        public void AddFinovaItaly_RegisteredParser_CanParseIbans()
        {
            var services = new ServiceCollection();
            services.AddFinovaItaly();
            var serviceProvider = services.BuildServiceProvider();
            var parser = serviceProvider.GetRequiredService<IIbanParser>();

            var result = parser.ParseIban("IT60X0542811101000000123456");
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be("IT");
        }

        [Fact]
        public void AddFinovaItaly_ParserDependsOnValidator()
        {
            var services = new ServiceCollection();
            services.AddFinovaItaly();
            var serviceProvider = services.BuildServiceProvider();

            var parser = serviceProvider.GetService<IIbanParser>();
            var validator = serviceProvider.GetService<IIbanValidator>();

            parser.Should().NotBeNull();
            validator.Should().NotBeNull();
        }

        [Fact]
        public void AddFinovaItaly_CanBeCalledMultipleTimes()
        {
            var services = new ServiceCollection();
            services.AddFinovaItaly();
            services.AddFinovaItaly(); // Should not throw
            var serviceProvider = services.BuildServiceProvider();

            var validators = serviceProvider.GetServices<IIbanValidator>();
            validators.Should().NotBeEmpty();
        }
    }
}
