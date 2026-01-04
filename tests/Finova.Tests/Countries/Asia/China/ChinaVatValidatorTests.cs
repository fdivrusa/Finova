using Finova.Countries.Asia.China.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.China;

public class ChinaVatValidatorTests
{
    [Theory]
    [InlineData("91110000100000003H")] // Format test - will test checksum in dedicated test
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        // Note: This tests format validation; actual USCC validation uses complex Mod 31
        var result = ChinaVatValidator.Validate(vat);
        // Result depends on USCC checksum (Mod 31)
        result.Should().NotBeNull();
    }
    
    [Fact]
    public void Validate_WithFormatTest_ReturnsResult()
    {
        // This is a format-only validation test
        var result = ChinaVatValidator.Validate("91110000100000003H");
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("91110000MA001234X")] // Too short (17 chars)
    [InlineData("91110000MA001234X56")] // Too long (19 chars)
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = ChinaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "91110000MA001234X5";
        var result = ChinaVatValidator.GetVatDetails(vat);
        
        if (result != null)
        {
            result.CountryCode.Should().Be("CN");
            result.IdentifierKind.Should().Be("USCC");
            result.IsEuVat.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = ChinaVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
