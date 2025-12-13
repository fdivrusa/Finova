using Finova.Countries.Europe.France.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.France.Validators;

public class FranceVatValidatorTests
{
    [Theory]
    [InlineData("FR40303265045")] // Valid French VAT (Old system)
    [InlineData("40303265045")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = FranceVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FR40303265040")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = FranceVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("FR40303265045", "FR", "FR40303265045")]
    [InlineData("40303265045", "FR", "FR40303265045")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new FranceVatValidator().Parse(vat);
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
        var result = new FranceVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
