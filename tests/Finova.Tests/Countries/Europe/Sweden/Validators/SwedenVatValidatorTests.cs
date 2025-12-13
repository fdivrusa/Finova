using Finova.Countries.Europe.Sweden.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Sweden.Validators;

public class SwedenVatValidatorTests
{
    [Theory]
    [InlineData("SE556728583701")] // Valid Swedish VAT (12 digits)
    [InlineData("556728583701")] // Without prefix
    [InlineData("SE123456789701")] // Valid format (Luhn valid on first 10 digits)
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = SwedenVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SE 5567 2858 3701")] // With spaces
    [InlineData("se556728583701")] // Lowercase
    [InlineData(" SE556728583701 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = SwedenVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("SE556728583001")] // Wrong - Luhn fails on first 10 digits (5567285830)
    [InlineData("SE55672858370")] // Too short (11 chars, need 12)
    [InlineData("SE5567285837011")] // Too long (13 chars)
    [InlineData("SE55672858370X")] // Contains letter in number
    [InlineData("XX556728583701")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        // Act
        var result = SwedenVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("SE556728583701", "SE", "556728583701")]
    [InlineData("556728583701", "SE", "556728583701")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        // Act
        var result = new SwedenVatValidator().Parse(vat);

        // Assert
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("SE556728583001")] // Invalid check digit (Luhn fails on 5567285830)
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        // Act
        var result = new SwedenVatValidator().Parse(vat);

        // Assert
        result.Should().BeNull();
    }
}
