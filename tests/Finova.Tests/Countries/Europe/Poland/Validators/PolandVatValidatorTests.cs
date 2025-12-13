using Finova.Countries.Europe.Poland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Poland.Validators;

public class PolandVatValidatorTests
{
    [Theory]
    [InlineData("PL5260300291")] // Valid (Oracle Polska)
    [InlineData("5260300291")] // Valid without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = PolandVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL 5260 3002 91")] // With spaces
    [InlineData(" PL5260300291 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        var result = PolandVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL5260300292")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("PL526030029")] // Too short
    [InlineData("XX5260300291")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = PolandVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("PL5260300291", "PL", "5260300291")]
    [InlineData("5260300291", "PL", "5260300291")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new PolandVatValidator().Parse(vat);
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
        var result = new PolandVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
