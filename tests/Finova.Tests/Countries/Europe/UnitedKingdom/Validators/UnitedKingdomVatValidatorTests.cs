using Finova.Countries.Europe.UnitedKingdom.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.UnitedKingdom.Validators;

public class UnitedKingdomVatValidatorTests
{
    [Theory]
    [InlineData("GB123456782")] // Valid UK VAT (9 digits)
    [InlineData("123456782")] // Without prefix
    [InlineData("GB999999973")] // Another valid example
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = UnitedKingdomVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("GB123456780")] // Invalid checksum
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = UnitedKingdomVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("GB123456782", "GB", "123456782")]
    [InlineData("123456782", "GB", "123456782")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new UnitedKingdomVatValidator().Parse(vat);
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
        var result = new UnitedKingdomVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
