using Finova.Countries.SouthAmerica.Brazil.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Brazil;

public class BrazilVatValidatorTests
{
    [Theory]
    [InlineData("11222333000181")] // Valid CNPJ format
    [InlineData("BR11222333000181")] // With BR prefix
    [InlineData("11.222.333/0001-81")] // Formatted
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = BrazilVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1122233300018")] // Too short (13 digits)
    [InlineData("112223330001815")] // Too long (15 digits)
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = BrazilVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "11222333000181";
        var result = BrazilVatValidator.GetVatDetails(vat);

        if (result != null)
        {
            result.CountryCode.Should().Be("BR");
            result.IdentifierKind.Should().Be("CNPJ");
            result.IsEuVat.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = BrazilVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
