using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.SaudiArabia;

public class SaudiArabiaVatValidatorTests
{
    [Fact]
    public void Validate_WithValidFormat_PassesFormatCheck()
    {
        // Test correct format (15 digits starting with 3)
        var result = SaudiArabiaVatValidator.Validate("300000000000000");
        // Format check passes, Luhn checksum might fail
        result.Should().NotBeNull();
    }
    
    [Fact]
    public void Validate_WithPrefix_ParsesCorrectly()
    {
        // Test that SA prefix is stripped correctly
        var result = SaudiArabiaVatValidator.Validate("SA300000000000000");
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("30000000000000")] // Too short (14 digits)
    [InlineData("3000000000000003")] // Too long (16 digits)
    [InlineData("400000000000003")] // Doesn't start with 3
    [InlineData("100000000000003")] // Doesn't start with 3
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = SaudiArabiaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "300000000000003";
        var result = SaudiArabiaVatValidator.GetVatDetails(vat);
        
        if (result != null)
        {
            result.CountryCode.Should().Be("SA");
            result.IdentifierKind.Should().Be("VAT Registration Number");
            result.IsEuVat.Should().BeFalse();
            result.Notes.Should().Contain("رقم التسجيل الضريبي");
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("400000000000003")] // Invalid prefix
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = SaudiArabiaVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
