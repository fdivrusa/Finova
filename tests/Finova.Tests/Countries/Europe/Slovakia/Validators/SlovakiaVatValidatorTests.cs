using Finova.Countries.Europe.Slovakia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovakia.Validators;

public class SlovakiaVatValidatorTests
{
    [Theory]
    [InlineData("SK2022749619")] // Valid Slovak VAT
    [InlineData("2022749619")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = SlovakiaVatValidator.ValidateVat(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SK2022749610")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = SlovakiaVatValidator.ValidateVat(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("SK2022749619", "SK", "2022749619")]
    [InlineData("2022749619", "SK", "2022749619")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new SlovakiaVatValidator().Parse(vat);
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
        var result = new SlovakiaVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
