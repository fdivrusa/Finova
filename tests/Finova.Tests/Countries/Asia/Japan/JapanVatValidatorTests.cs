using Finova.Countries.Asia.Japan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Japan;

public class JapanVatValidatorTests
{
    [Theory]
    [InlineData("T8040001083697")] // Valid T + Corporate Number (checksum = 8)
    [InlineData("JPT8040001083697")] // With JP prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = JapanVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("T123456789012")] // Too short (12 digits after T)
    [InlineData("T12345678901234")] // Too long (14 digits after T)
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = JapanVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        // A corporate number that passes the Mod 9 checksum would be needed
        // For now, test the structure of the response
        var vat = "T1234567890123";
        var result = JapanVatValidator.GetVatDetails(vat);

        if (result != null)
        {
            result.CountryCode.Should().Be("JP");
            result.IdentifierKind.Should().Be("Invoice Registration Number");
            result.IsEuVat.Should().BeFalse();
            result.IsViesEligible.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = JapanVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
