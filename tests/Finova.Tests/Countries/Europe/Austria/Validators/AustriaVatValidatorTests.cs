using Finova.Countries.Europe.Austria.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Austria.Validators;

public class AustriaVatValidatorTests
{
    [Theory]
    [InlineData("ATU13585627")] // Valid Austrian VAT (9 digits with U)
    [InlineData("U13585627")] // Without prefix
    [InlineData("ATU12345675")] // Valid example
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = AustriaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AT U1358 5627")] // With spaces
    [InlineData("atu13585627")] // Lowercase
    [InlineData(" ATU13585627 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        var result = AustriaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ATU13585620")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ATU1358562")] // Too short
    [InlineData("ATU135856278")] // Too long
    [InlineData("AT1358562")] // Missing U (should be added, but invalid checksum)
    [InlineData("XX13585627")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = AustriaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("ATU13585627", "AT", "ATU13585627")]
    [InlineData("U13585627", "AT", "ATU13585627")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new AustriaVatValidator().Parse(vat);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ATU13585620")] // Invalid checksum
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = new AustriaVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
