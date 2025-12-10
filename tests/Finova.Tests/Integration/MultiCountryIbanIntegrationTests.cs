using Finova.Core.Bic;
using Finova.Core.Iban;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Germany.Models;
using Finova.Countries.Europe.Italy.Models;
using Finova.Countries.Europe.Netherlands.Models;
using Finova.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Integration;

public class EuropeIbanIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    public EuropeIbanIntegrationTests()
    {
        var services = new ServiceCollection();

        // 1. Act: Register the unified library
        services.AddFinova();

        _serviceProvider = services.BuildServiceProvider();
    }

    #region DI Registration Tests

    [Fact]
    public void AddFinova_Registers_Unified_Parser_Only()
    {
        // Act
        var parsers = _serviceProvider.GetServices<IIbanParser>().ToList();

        // Assert
        // We expect exactly ONE parser now: The "EuropeIbanParser" wrapper.
        // It internally handles all countries.
        parsers.Should().HaveCount(1);
        parsers.First().CountryCode.Should().BeNull(); // The wrapper code we defined earlier
    }

    [Fact]
    public void AddFinova_Registers_Core_Validators()
    {
        // Assert that Bic/Card validators are also registered by the main extension method
        _serviceProvider.GetService<IBicValidator>().Should().NotBeNull();
        _serviceProvider.GetService<IPaymentCardValidator>().Should().NotBeNull();
        _serviceProvider.GetService<IPaymentReferenceValidator>().Should().NotBeNull();
    }

    #endregion

    #region Router Validation Logic (The "Smart" Tests)

    [Theory]
    // Tier 1 Countries (Specific Rules Implemented)
    [InlineData("BE68539007547034", "BE", true)]
    [InlineData("FR1420041010050500013M02606", "FR", true)]
    [InlineData("DE89370400440532013000", "DE", true)]
    [InlineData("IT60X0542811101000000123456", "IT", true)]
    [InlineData("ES9121000418450200051332", "ES", true)]
    // Tier 2 Countries (Generic Fallback)
    [InlineData("NL91ABNA0417164300", "NL", true)]
    [InlineData("LU280019400644750000", "LU", true)]
    [InlineData("GB29NWBK60161331926819", "GB", true)]
    // Invalid Cases
    [InlineData("BE68539007547035", "BE", false)] // Wrong Checksum
    [InlineData("IT60X0542811101000000123457", "IT", false)] // Wrong Checksum
    public void Parser_RoutesToCorrectLogic_AndValidates(string iban, string expectedCountry, bool expectedValid)
    {
        // Arrange
        var parser = _serviceProvider.GetRequiredService<IIbanParser>();

        // Act
        var result = parser.ParseIban(iban);

        // Assert
        if (expectedValid)
        {
            result.Should().NotBeNull();
            result!.IsValid.Should().BeTrue();
            result.CountryCode.Should().Be(expectedCountry);
        }
        else
        {
            // The parser returns null for invalid IBANs (based on your implementation)
            result.Should().BeNull();
        }
    }

    #endregion

    #region Polymorphic Return Types (The "Power" Tests)

    [Fact]
    public void Parse_GermanIban_Returns_GermanyDetails()
    {
        // Arrange
        var parser = _serviceProvider.GetRequiredService<IIbanParser>();
        var germanIban = "DE89370400440532013000";

        // Act
        var result = parser.ParseIban(germanIban);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<GermanyIbanDetails>(); // Verify we got the specific class

        var details = (GermanyIbanDetails)result!;
        details.Bankleitzahl.Should().Be("37040044"); // Specific German property
    }

    [Fact]
    public void Parse_ItalianIban_Returns_ItalyDetails()
    {
        // Arrange
        var parser = _serviceProvider.GetRequiredService<IIbanParser>();
        var italianIban = "IT60X0542811101000000123456";

        // Act
        var result = parser.ParseIban(italianIban);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ItalyIbanDetails>();

        var details = (ItalyIbanDetails)result!;
        details.Abi.Should().Be("05428"); // Specific Italian property
        details.Cin.Should().Be("X");
    }

    [Fact]
    public void Parse_DutchIban_Returns_GenericDetails()
    {
        // Arrange
        var parser = _serviceProvider.GetRequiredService<IIbanParser>();
        var dutchIban = "NL91ABNA0417164300"; // NL uses generic fallback currently

        // Act
        var result = parser.ParseIban(dutchIban);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NetherlandsIbanDetails>(); // Should just be the base class
        result!.CountryCode.Should().Be("NL");
    }

    #endregion
}
