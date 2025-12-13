using Finova.Countries.Europe.Denmark.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Denmark.Validators;

public class DenmarkVatValidatorTests
{
    [Theory]
    [InlineData("DK13585628")] // Valid Danish VAT (8 digits)
    [InlineData("13585628")] // Without prefix
    [InlineData("DK88146328")] // Another valid example
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = DenmarkVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("DK 1358 5628")] // With spaces
    [InlineData("dk13585628")] // Lowercase
    [InlineData(" DK13585628 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        var result = DenmarkVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("DK13585620")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("DK1358562")] // Too short
    [InlineData("DK135856289")] // Too long
    [InlineData("DK1358562X")] // Contains letter in number
    [InlineData("XX13585628")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = DenmarkVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("DK13585628", "DK", "13585628")]
    [InlineData("13585628", "DK", "13585628")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new DenmarkVatValidator().Parse(vat);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("DK13585620")] // Invalid checksum
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = new DenmarkVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
