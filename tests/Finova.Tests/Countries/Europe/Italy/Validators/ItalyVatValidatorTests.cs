using Finova.Countries.Europe.Italy.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Italy.Validators;

public class ItalyVatValidatorTests
{
    [Theory]
    [InlineData("IT12345678903")] // Valid Italian VAT (11 digits)
    [InlineData("12345678903")] // Without prefix
    [InlineData("IT00743110157")] // Another valid example
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = ItalyVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IT 1234 5678 903")] // With spaces
    [InlineData("it12345678903")] // Lowercase
    [InlineData(" IT12345678903 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        var result = ItalyVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IT12345678900")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("IT1234567890")] // Too short
    [InlineData("IT123456789012")] // Too long
    [InlineData("IT1234567890X")] // Contains letter in number
    [InlineData("XX12345678903")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = ItalyVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("IT12345678903", "IT", "IT12345678903")]
    [InlineData("12345678903", "IT", "IT12345678903")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new ItalyVatValidator().Parse(vat);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("IT12345678900")] // Invalid checksum
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = new ItalyVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
