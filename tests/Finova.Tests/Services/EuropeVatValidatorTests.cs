using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class EuropeVatValidatorTests
{
    [Theory]
    [InlineData("BE0403170701", true)] // Valid Belgium VAT
    [InlineData("BE0123456700", false)] // Invalid Belgium VAT
    [InlineData("FR61954506077", true)] // Valid France VAT
    [InlineData("FR00000000000", false)] // Invalid France VAT
    [InlineData("IT06363391001", true)] // Valid Italy VAT
    [InlineData("IT00000000000", false)] // Invalid Italy VAT
    public void ValidateVat_WithSupportedCountries_DelegatesCorrectly(string vat, bool expectedIsValid)
    {
        // Act
        var result = EuropeVatValidator.ValidateVat(vat);

        // Assert
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void ValidateVat_WithUnsupportedCountry_ReturnsFailure()
    {
        // Arrange
        var vat = "DE123456789"; // Germany not yet implemented

        // Act
        var result = EuropeVatValidator.ValidateVat(vat);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidCountryCode);
    }

    [Fact]
    public void ValidateVat_WithEmptyInput_ReturnsFailure()
    {
        // Act
        var result = EuropeVatValidator.ValidateVat("");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }
}
