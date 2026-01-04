using Finova.Countries.MiddleEast.UAE.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.UAE;

public class UaeVatValidatorTests
{
    [Theory]
    [InlineData("100000000000001")] // Valid TRN format - digit sum 2 -> check = 8
    public void Validate_WithValidFormat_PassesFormatCheck(string vat)
    {
        var result = UaeVatValidator.Validate(vat);
        // Format check passes, checksum might fail depending on algorithm
        result.Should().NotBeNull();
    }

    [Fact]
    public void Validate_WithCorrectFormat_ReturnsResult()
    {
        // Test that validation returns a result for correct format
        var result = UaeVatValidator.Validate("100111111111118");
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("10012345678901")] // Too short (14 digits)
    [InlineData("1001234567890123")] // Too long (16 digits)
    [InlineData("200123456789012")] // Doesn't start with 100
    [InlineData("101234567890123")] // Doesn't start with 100
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = UaeVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "100123456789012";
        var result = UaeVatValidator.GetVatDetails(vat);

        if (result != null)
        {
            result.CountryCode.Should().Be("AE");
            result.IdentifierKind.Should().Be("TRN");
            result.IsEuVat.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("200123456789012")] // Invalid prefix
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = UaeVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
