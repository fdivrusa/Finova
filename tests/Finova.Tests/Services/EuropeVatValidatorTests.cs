using Finova.Services;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Services;

public class EuropeVatValidatorTests
{
    [Theory]
    [InlineData("RO 18547290")]
    [InlineData("RO-18547290")]
    [InlineData("RO.18547290")]
    public void Validate_ShouldHandleSanitization(string vatNumber)
    {
        var result = EuropeVatValidator.ValidateVat(vatNumber);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("FR", "FR12345678901")] // France
    [InlineData("BE", "BE0123456789")] // Belgium
    [InlineData("AT", "ATU12345678")] // Austria
    [InlineData("AL", "ALJ12345678L")] // Albania
    [InlineData("AD", "ADF123456A")] // Andorra
    [InlineData("IT", "IT12345678901")] // Italy
    [InlineData("AZ", "AZ1234567890")] // Azerbaijan
    [InlineData("BY", "BY123456789")] // Belarus
    [InlineData("BA", "BA1234567890123")] // Bosnia and Herzegovina
    [InlineData("BG", "BG123456789")] // Bulgaria
    [InlineData("HR", "HR12345678901")] // Croatia
    [InlineData("CY", "CY12345678A")] // Cyprus
    [InlineData("CZ", "CZ12345678")] // Czech Republic
    [InlineData("DK", "DK12345678")] // Denmark
    [InlineData("EE", "EE123456789")] // Estonia
    [InlineData("FO", "FO123456")] // Faroe Islands
    [InlineData("FI", "FI12345678")] // Finland
    [InlineData("GE", "GE123456789")] // Georgia
    [InlineData("DE", "DE123456789")] // Germany
    [InlineData("EL", "EL123456789")] // Greece
    [InlineData("HU", "HU12345678")] // Hungary
    [InlineData("IS", "IS123456")] // Iceland
    [InlineData("IE", "IE1234567T")] // Ireland
    [InlineData("LV", "LV12345678901")] // Latvia
    [InlineData("LI", "LI12345")] // Liechtenstein
    [InlineData("LT", "LT123456789")] // Lithuania
    [InlineData("LU", "LU12345678")] // Luxembourg
    [InlineData("MT", "MT12345678")] // Malta
    [InlineData("MD", "MD1234567890123")] // Moldova
    [InlineData("MC", "MC12345678901")] // Monaco (FR format)
    [InlineData("ME", "ME12345678")] // Montenegro
    [InlineData("NL", "NL123456789B01")] // Netherlands
    [InlineData("MK", "MK1234567890123")] // North Macedonia
    [InlineData("NO", "NO123456789")] // Norway
    [InlineData("PL", "PL1234567890")] // Poland
    [InlineData("PT", "PT123456789")] // Portugal
    [InlineData("RO", "RO1234567890")] // Romania
    [InlineData("RS", "RS123456789")] // Serbia
    [InlineData("SK", "SK1234567890")] // Slovakia
    [InlineData("SI", "SI12345678")] // Slovenia
    [InlineData("ES", "ES12345678Z")] // Spain
    [InlineData("SE", "SE123456789012")] // Sweden
    [InlineData("CHE", "CHE123456789")] // Switzerland
    [InlineData("TR", "TR1234567890")] // Turkey
    [InlineData("UA", "UA123456789")] // Ukraine
    [InlineData("GB", "GB123456789")] // United Kingdom
    public void Validate_ShouldDelegateToCorrectValidator(string countryCode, string vatNumber)
    {
        // Act
        var result = EuropeVatValidator.ValidateVat(vatNumber);

        // Assert
        // If the country is supported, we shouldn't get UnsupportedCountry error.
        Assert.DoesNotContain(result.Errors, e => e.Code == ValidationErrorCode.UnsupportedCountry);
        Assert.StartsWith(countryCode, vatNumber);
    }

    [Fact]
    public void Validate_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        // Arrange
        var vatNumber = "XX123456789";

        // Act
        var result = EuropeVatValidator.ValidateVat(vatNumber);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.UnsupportedCountry, result.Errors[0].Code);
    }

    [Fact]
    public void Validate_ShouldReturnInvalidInput_ForEmptyInput()
    {
        var result = EuropeVatValidator.ValidateVat("");
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("RO", "18547290")] // Valid RO without prefix
    [InlineData("DE", "123456788")] // Valid DE without prefix
    [InlineData("GB", "123456782")] // Valid GB without prefix
    public void Validate_WithExplicitCountryCode_ShouldValidateWithoutPrefix(string countryCode, string vatNumber)
    {
        var result = EuropeVatValidator.ValidateVat(vatNumber, countryCode);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("RO", "RO18547290")] // Valid RO with prefix + explicit code
    [InlineData("DE", "DE123456788")] // Valid DE with prefix + explicit code
    public void Validate_WithExplicitCountryCode_ShouldValidateWithPrefix(string countryCode, string vatNumber)
    {
        var result = EuropeVatValidator.ValidateVat(vatNumber, countryCode);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithExplicitCountryCode_ShouldFailForInvalidFormat()
    {
        var result = EuropeVatValidator.ValidateVat("1234567891011", "RO"); // Invalid RO
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WithoutCountryCode_ShouldFailIfPrefixMissing()
    {
        var result = EuropeVatValidator.ValidateVat("1234567890"); // No prefix, no explicit code
        // This might fail with UnsupportedCountry or InvalidInput depending on length check
        // "12" is extracted as country code "12", which is unsupported.
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.UnsupportedCountry, result.Errors[0].Code);
    }
}
