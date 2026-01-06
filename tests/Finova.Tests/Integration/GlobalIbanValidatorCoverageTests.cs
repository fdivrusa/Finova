using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Integration;

public class GlobalIbanValidatorCoverageTests
{
    [Theory]
    [InlineData("DZ")] // Algeria
    [InlineData("AO")] // Angola
    [InlineData("BJ")] // Benin
    [InlineData("BF")] // Burkina Faso
    [InlineData("BI")] // Burundi
    [InlineData("CM")] // Cameroon
    [InlineData("CV")] // Cape Verde
    [InlineData("CF")] // Central African Republic
    [InlineData("TD")] // Chad
    [InlineData("KM")] // Comoros
    [InlineData("CG")] // Congo
    [InlineData("CI")] // Cote d'Ivoire
    [InlineData("DJ")] // Djibouti
    [InlineData("GQ")] // Equatorial Guinea
    [InlineData("GA")] // Gabon
    [InlineData("GW")] // Guinea-Bissau
    [InlineData("LY")] // Libya
    [InlineData("MG")] // Madagascar
    [InlineData("ML")] // Mali
    [InlineData("MZ")] // Mozambique
    [InlineData("NE")] // Niger
    [InlineData("ST")] // Sao Tome and Principe
    [InlineData("SN")] // Senegal
    [InlineData("SC")] // Seychelles
    [InlineData("SO")] // Somalia
    [InlineData("SD")] // Sudan
    [InlineData("TG")] // Togo
    [InlineData("HN")] // Honduras
    [InlineData("NI")] // Nicaragua
    [InlineData("LC")] // Saint Lucia
    [InlineData("IQ")] // Iraq
    [InlineData("YE")] // Yemen
    public void ValidateIban_ShouldRouteToSpecificValidator_ForNewCountries(string countryCode)
    {
        // Arrange
        // We construct a string that looks like an IBAN (Country Code + Check Digits + some length)
        // Length doesn't matter for routing check, but we make it long enough to avoid immediate length checks in some logic if possible, 
        // though specific validators usually check length first.
        // Even if it returns InvalidLength, it proves it routed to the specific validator (because generic fallback returns UnsupportedCountry for invalid generic ibans)

        string input = $"{countryCode}00123456789012345678901234567890"; // 34 chars max length

        // Act
        var result = GlobalIbanValidator.ValidateIban(input);

        // Assert
        // We expect any error EXCEPT UnsupportedCountry.
        // If the country was not registered in the switch case, it would fall to default.
        // Since input is invalid generic IBAN (checksum wrong), default returns UnsupportedCountry.
        // If it IS registered, it goes to SpecificValidator.
        // SpecificValidator returns InvalidLength, InvalidFormat, or InvalidChecksum.

        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry,
            $"Country {countryCode} should be routed to a specific validator, but fell back to default.");
    }
}
