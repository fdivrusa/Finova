using Finova.Countries.Europe.Norway.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Norway.Validators;

public class NorwayVatValidatorTests
{
    [Theory]
    [InlineData("NO995567636")] // Valid Norwegian VAT
    [InlineData("995567636")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = NorwayVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NO 9955 6763 6")] // With spaces
    [InlineData(" NO995567636 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        var result = NorwayVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("NO995567630")] // Invalid checksum
    [InlineData("NO12345678")] // Too short
    [InlineData("NO1234567890")] // Too long
    [InlineData("XX995567636")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = NorwayVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("NO995567636", "NO", "995567636")]
    [InlineData("995567636", "NO", "995567636")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new NorwayVatValidator().Parse(vat);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = new NorwayVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
