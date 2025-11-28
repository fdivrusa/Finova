using Finova.Belgium.Extensions;
using Finova.Core.Extensions;
using Finova.Core.Interfaces;
using Finova.France.Extensions;
using Finova.Germany.Extensions;
using Finova.Italy.Extensions;
using Finova.Luxembourg.Extensions;
using Finova.Netherlands.Extensions;
using Finova.Spain.Extensions;
using Finova.UnitedKingdom.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Integration
{
    public class MultiCountryIbanIntegrationTests
    {
        private readonly IServiceProvider _serviceProvider;

        public MultiCountryIbanIntegrationTests()
        {
            var services = new ServiceCollection();

            // Register all country services
            services.AddFinovaCoreServices();
            services.AddFinovaBelgium();
            services.AddFinovaFrance();
            services.AddFinovaGermany();
            services.AddFinovaNetherlands();
            services.AddFinovaLuxembourg();
            services.AddFinovaUnitedKingdom();
            services.AddFinovaItaly();
            services.AddFinovaSpain();

            _serviceProvider = services.BuildServiceProvider();
        }

        #region Validator Registration Tests

        [Fact]
        public void AllCountries_RegisterValidators()
        {
            // Act
            var validators = _serviceProvider.GetServices<IIbanValidator>().ToList();

            // Assert
            validators.Should().HaveCount(8);
            validators.Should().Contain(v => v.CountryCode == "BE");
            validators.Should().Contain(v => v.CountryCode == "FR");
            validators.Should().Contain(v => v.CountryCode == "DE");
            validators.Should().Contain(v => v.CountryCode == "NL");
            validators.Should().Contain(v => v.CountryCode == "LU");
            validators.Should().Contain(v => v.CountryCode == "GB");
            validators.Should().Contain(v => v.CountryCode == "IT");
            validators.Should().Contain(v => v.CountryCode == "ES");
        }

        [Fact]
        public void AllCountries_RegisterParsers()
        {
            // Act
            var parsers = _serviceProvider.GetServices<IIbanParser>().ToList();

            // Assert
            parsers.Should().HaveCount(9);
            parsers.Should().Contain(p => p.CountryCode == null);
            parsers.Should().Contain(p => p.CountryCode == "BE");
            parsers.Should().Contain(p => p.CountryCode == "FR");
            parsers.Should().Contain(p => p.CountryCode == "DE");
            parsers.Should().Contain(p => p.CountryCode == "NL");
            parsers.Should().Contain(p => p.CountryCode == "LU");
            parsers.Should().Contain(p => p.CountryCode == "GB");
            parsers.Should().Contain(p => p.CountryCode == "IT");
            parsers.Should().Contain(p => p.CountryCode == "ES");

        }

        #endregion

        #region Multi-Country Validation Tests

        [Theory]
        [InlineData("BE68539007547034", "BE")]
        [InlineData("FR1420041010050500013M02606", "FR")]
        [InlineData("DE89370400440532013000", "DE")]
        [InlineData("NL91ABNA0417164300", "NL")]
        [InlineData("LU280019400644750000", "LU")]
        [InlineData("GB29NWBK60161331926819", "GB")]
        [InlineData("IT60X0542811101000000123456", "IT")]
        [InlineData("ES9121000418450200051332", "ES")]
        public void ValidateIban_WithAllCountries_WorksCorrectly(string iban, string expectedCountry)
        {
            // Arrange
            var validators = _serviceProvider.GetServices<IIbanValidator>();
            var validator = validators.FirstOrDefault(v => v.CountryCode == expectedCountry);

            // Act
            var result = validator?.IsValidIban(iban);

            // Assert
            result.Should().BeTrue($"{expectedCountry} IBAN should be valid");
        }

        #endregion

        #region Multi-Country Parsing Tests

        [Theory]
        [InlineData("BE68539007547034", "BE")]
        [InlineData("FR1420041010050500013M02606", "FR")]
        [InlineData("DE89370400440532013000", "DE")]
        [InlineData("NL91ABNA0417164300", "NL")]
        [InlineData("LU280019400644750000", "LU")]
        [InlineData("GB29NWBK60161331926819", "GB")]
        [InlineData("IT60X0542811101000000123456", "IT")]
        [InlineData("ES9121000418450200051332", "ES")]
        public void ParseIban_WithAllCountries_WorksCorrectly(string iban, string expectedCountry)
        {
            // Arrange
            var parsers = _serviceProvider.GetServices<IIbanParser>();
            var parser = parsers.FirstOrDefault(p => p.CountryCode == expectedCountry);

            // Act
            var result = parser?.ParseIban(iban);

            // Assert
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be(expectedCountry);
            result.IsValid.Should().BeTrue();
        }

        #endregion

        #region Generic Service Tests

        [Fact]
        public void GenericIbanService_CanValidateAllCountries()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<IIbanService>();

            // Act & Assert
            service.IsValidIban("BE68539007547034").Should().BeTrue();
            service.IsValidIban("FR1420041010050500013M02606").Should().BeTrue();
            service.IsValidIban("DE89370400440532013000").Should().BeTrue();
            service.IsValidIban("NL91ABNA0417164300").Should().BeTrue();
            service.IsValidIban("LU280019400644750000").Should().BeTrue();
            service.IsValidIban("GB29NWBK60161331926819").Should().BeTrue();
            service.IsValidIban("IT60X0542811101000000123456").Should().BeTrue();
            service.IsValidIban("ES9121000418450200051332").Should().BeTrue();
        }

        [Fact]
        public void GenericIbanService_CanFormatAllCountries()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<IIbanService>();

            // Act & Assert
            service.FormatIban("BE68539007547034").Should().Be("BE68 5390 0754 7034");
            service.FormatIban("FR1420041010050500013M02606").Should().Be("FR14 2004 1010 0505 0001 3M02 606");
            service.FormatIban("DE89370400440532013000").Should().Be("DE89 3704 0044 0532 0130 00");
            service.FormatIban("IT60X0542811101000000123456").Should().Be("IT60 X054 2811 1010 0000 0123 456");
            service.FormatIban("ES9121000418450200051332").Should().Be("ES91 2100 0418 4502 0005 1332");
        }

        #endregion

        #region Country Routing Tests

        [Theory]
        [InlineData("BE68539007547034", "BE")]
        [InlineData("FR1420041010050500013M02606", "FR")]
        [InlineData("DE89370400440532013000", "DE")]
        [InlineData("NL91ABNA0417164300", "NL")]
        [InlineData("LU280019400644750000", "LU")]
        [InlineData("GB29NWBK60161331926819", "GB")]
        [InlineData("IT60X0542811101000000123456", "IT")]
        [InlineData("ES9121000418450200051332", "ES")]
        public void CanRouteToCorrectCountryValidator(string iban, string expectedCountry)
        {
            // Arrange
            var genericService = _serviceProvider.GetRequiredService<IIbanService>();
            var validators = _serviceProvider.GetServices<IIbanValidator>();
            var countryCode = genericService.GetCountryCode(iban);

            // Act
            var validator = validators.FirstOrDefault(v => v.CountryCode == countryCode);

            // Assert
            validator.Should().NotBeNull();
            validator!.CountryCode.Should().Be(expectedCountry);
            validator.IsValidIban(iban).Should().BeTrue();
        }

        [Theory]
        [InlineData("BE68539007547034", "BE")]
        [InlineData("FR1420041010050500013M02606", "FR")]
        [InlineData("DE89370400440532013000", "DE")]
        [InlineData("NL91ABNA0417164300", "NL")]
        [InlineData("LU280019400644750000", "LU")]
        [InlineData("GB29NWBK60161331926819", "GB")]
        [InlineData("IT60X0542811101000000123456", "IT")]
        [InlineData("ES9121000418450200051332", "ES")]
        public void CanRouteToCorrectCountryParser(string iban, string expectedCountry)
        {
            // Arrange
            var genericService = _serviceProvider.GetRequiredService<IIbanService>();
            var parsers = _serviceProvider.GetServices<IIbanParser>();
            var countryCode = genericService.GetCountryCode(iban);

            // Act
            var parser = parsers.FirstOrDefault(p => p.CountryCode == countryCode);
            var result = parser?.ParseIban(iban);

            // Assert
            parser.Should().NotBeNull();
            parser!.CountryCode.Should().Be(expectedCountry);
            result.Should().NotBeNull();
            result!.CountryCode.Should().Be(expectedCountry);
        }

        #endregion

        #region Cross-Country Validation Tests

        [Theory]
        [InlineData("BE68539007547034", "FR")] // Belgian IBAN with French validator
        [InlineData("FR1420041010050500013M02606", "DE")] // French IBAN with German validator
        [InlineData("DE89370400440532013000", "NL")] // German IBAN with Dutch validator
        public void ValidatorRejectsWrongCountryIban(string iban, string wrongCountryCode)
        {
            // Arrange
            var validators = _serviceProvider.GetServices<IIbanValidator>();
            var wrongValidator = validators.First(v => v.CountryCode == wrongCountryCode);

            // Act
            var result = wrongValidator.IsValidIban(iban);

            // Assert
            result.Should().BeFalse($"{wrongCountryCode} validator should reject IBANs from other countries");
        }

        #endregion

        #region Service Lifecycle Tests

        [Fact]
        public void AllServices_AreSingletons()
        {
            // Arrange & Act
            var service1 = _serviceProvider.GetService<IIbanService>();
            var service2 = _serviceProvider.GetService<IIbanService>();

            var validators1 = _serviceProvider.GetServices<IIbanValidator>().ToList();
            var validators2 = _serviceProvider.GetServices<IIbanValidator>().ToList();

            var parsers1 = _serviceProvider.GetServices<IIbanParser>().ToList();
            var parsers2 = _serviceProvider.GetServices<IIbanParser>().ToList();

            // Assert
            service1.Should().BeSameAs(service2);

            for (int i = 0; i < validators1.Count; i++)
            {
                validators1[i].Should().BeSameAs(validators2[i]);
            }

            for (int i = 0; i < parsers1.Count; i++)
            {
                parsers1[i].Should().BeSameAs(parsers2[i]);
            }
        }

        #endregion
    }
}
