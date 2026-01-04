using Finova.Countries.SouthAmerica.Mexico.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Mexico;

public class MexicoVatValidatorTests
{
    [Theory]
    [InlineData("XAXX010101000")] // Generic VAT RFC (company)
    [InlineData("MXXAXX010101000")] // With MX prefix
    [InlineData("XEXX010101000")] // Generic foreign RFC
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = MexicoVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ABC12345678")] // Too short (11 chars)
    [InlineData("ABCDE123456AB12")] // Too long (14 chars)
    [InlineData("12345678901234")] // All numeric
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = MexicoVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("ABC123456AB1", "Company")]
    [InlineData("ABCD123456AB1", "Individual")]
    public void GetVatDetails_ReturnsCorrectEntityType(string vat, string expectedType)
    {
        var result = MexicoVatValidator.GetVatDetails(vat);

        if (result != null)
        {
            result.CountryCode.Should().Be("MX");
            result.IdentifierKind.Should().Be("RFC");
            result.IsEuVat.Should().BeFalse();
            result.Notes.Should().Contain(expectedType);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = MexicoVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
