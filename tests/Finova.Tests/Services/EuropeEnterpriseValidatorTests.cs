using Finova.Services;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Services;

public class EuropeEnterpriseValidatorTests
{
    [Theory]
    [InlineData("BE", "BE0123456789")] // Belgium
    [InlineData("AT", "AT123456x")] // Austria (AT prefix stripped)
    [InlineData("FR", "FR12345678901234")] // France (FR prefix stripped)
    [InlineData("FR", "FR732829320")] // France SIREN
    [InlineData("FR", "FR73282932000074")] // France SIRET
    public void Validate_ShouldDelegateToCorrectValidator(string countryCode, string number)
    {
        // Act
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(number);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.Code == ValidationErrorCode.UnsupportedCountry);
        Assert.StartsWith(countryCode, number);
    }

    [Fact]
    public void Validate_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        // Arrange
        var number = "XX123456789";

        // Act
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(number);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.UnsupportedCountry, result.Errors[0].Code);
    }

    [Fact]
    public void Validate_WithEnum_ShouldDelegateCorrectly()
    {
        // Valid SIREN
        var resultSiren = EuropeEnterpriseValidator.ValidateEnterpriseNumber("732 829 320", Finova.Core.Enterprise.EnterpriseNumberType.FranceSiren);
        Assert.True(resultSiren.IsValid);

        // Valid SIRET
        var resultSiret = EuropeEnterpriseValidator.ValidateEnterpriseNumber("732 829 320 00074", Finova.Core.Enterprise.EnterpriseNumberType.FranceSiret);
        Assert.True(resultSiret.IsValid);

        // Invalid SIREN as SIRET
        var resultInvalid = EuropeEnterpriseValidator.ValidateEnterpriseNumber("732 829 320", Finova.Core.Enterprise.EnterpriseNumberType.FranceSiret);
        Assert.False(resultInvalid.IsValid);
    }

    [Fact]
    public void Validate_ShouldReturnInvalidInput_ForEmptyInput()
    {
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber("");
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.Errors[0].Code);
    }
    [Theory]
    [InlineData("AT", "123456x")] // Austria (no prefix)
    [InlineData("AT", "AT123456x")] // Austria (with prefix)
    [InlineData("BE", "0123456789")] // Belgium (no prefix)
    [InlineData("BE", "BE0123456789")] // Belgium (with prefix)
    [InlineData("FR", "732829320")] // France SIREN (no prefix)
    [InlineData("FR", "FR732829320")] // France SIREN (with prefix)
    [InlineData("FR", "73282932000074")] // France SIRET (no prefix)
    [InlineData("FR", "FR73282932000074")] // France SIRET (with prefix)
    public void Validate_WithCountryCode_ShouldDelegateToCorrectValidator(string countryCode, string number)
    {
        // Act
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(number, countryCode);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void Validate_WithCountryCode_ShouldReturnError_ForInvalidInputs()
    {
        // Empty country code
        var resultEmpty = EuropeEnterpriseValidator.ValidateEnterpriseNumber("123456", "");
        Assert.False(resultEmpty.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, resultEmpty.Errors[0].Code);

        // Unsupported country code
        var resultUnsupported = EuropeEnterpriseValidator.ValidateEnterpriseNumber("123456", "XX");
        Assert.False(resultUnsupported.IsValid);
        Assert.Equal(ValidationErrorCode.UnsupportedCountry, resultUnsupported.Errors[0].Code);
    }

    [Theory]
    [InlineData("123456x", Finova.Core.Enterprise.EnterpriseNumberType.AustriaFirmenbuch)]
    [InlineData("0123456789", Finova.Core.Enterprise.EnterpriseNumberType.BelgiumEnterpriseNumber)]
    public void Validate_WithType_ShouldDelegateCorrectly_ForAllTypes(string number, Finova.Core.Enterprise.EnterpriseNumberType type)
    {
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(number, type);
        Assert.DoesNotContain(result.Errors, e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void Validate_WithType_ShouldReturnError_ForInvalidInputs()
    {
        // Empty number
        var resultEmpty = EuropeEnterpriseValidator.ValidateEnterpriseNumber("", Finova.Core.Enterprise.EnterpriseNumberType.AustriaFirmenbuch);
        Assert.False(resultEmpty.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, resultEmpty.Errors[0].Code);

        // Unsupported enum value (casting an int to enum)
        var resultUnsupported = EuropeEnterpriseValidator.ValidateEnterpriseNumber("123", (Finova.Core.Enterprise.EnterpriseNumberType)999);
        Assert.False(resultUnsupported.IsValid);
        Assert.Equal(ValidationErrorCode.UnsupportedCountry, resultUnsupported.Errors[0].Code);
    }
}
