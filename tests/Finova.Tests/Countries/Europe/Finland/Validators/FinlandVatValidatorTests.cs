using Finova.Countries.Europe.Finland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Finland.Validators;

public class FinlandVatValidatorTests
{
    [Theory]
    [InlineData("FI20774740")] // Valid Finnish VAT
    [InlineData("20774740")] // Without prefix
    [InlineData("FI12345671")] // Valid Finnish VAT
    [InlineData("12345671")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = FinlandVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FI 2077 4740")] // With spaces
    [InlineData("fi20774740")] // Lowercase
    [InlineData(" FI20774740 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = FinlandVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("FI20774741")] // Wrong check digit
    [InlineData("FI2077474")] // Too short
    [InlineData("FI207747401")] // Too long
    [InlineData("FI2077474X")] // Contains letter in number
    [InlineData("XX20774740")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        // Act
        var result = FinlandVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("FI20774740", "FI", "20774740")]
    [InlineData("20774740", "FI", "20774740")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        // Act
        var result = new FinlandVatValidator().Parse(vat);

        // Assert
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("FI20774741")] // Invalid check digit
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        // Act
        var result = new FinlandVatValidator().Parse(vat);

        // Assert
        result.Should().BeNull();
    }
}
