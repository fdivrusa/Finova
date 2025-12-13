using Finova.Countries.Europe.Germany.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany.Validators;

public class GermanyVatValidatorTests
{
    [Theory]
    [InlineData("DE123456788")] // Valid (Calculated: 12345678 -> Check 8)
    [InlineData("123456788")] // Valid without prefix
    [InlineData("DE811569869")] // Another valid example
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = GermanyVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("DE 1234 5678 8")] // With spaces
    [InlineData("de123456788")] // Lowercase
    [InlineData(" DE123456788 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        var result = GermanyVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("DE123456789")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("DE12345678")] // Too short
    [InlineData("DE1234567890")] // Too long
    [InlineData("DE12345678X")] // Contains letter in number
    [InlineData("XX123456788")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = GermanyVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("DE123456788", "DE", "123456788")]
    [InlineData("123456788", "DE", "123456788")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new GermanyVatValidator().Parse(vat);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("DE123456789")] // Invalid checksum
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = new GermanyVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
