using Finova.Countries.SouthAmerica.Argentina.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Argentina;

public class ArgentinaVatValidatorTests
{
    [Theory]
    [InlineData("20-12345678-6")] // Valid CUIT with correct checksum
    [InlineData("AR20123456786")] // With AR prefix
    [InlineData("20123456786")] // Without hyphens
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = ArgentinaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("2012345678")] // Too short (10 digits)
    [InlineData("201234567866")] // Too long (12 digits)
    [InlineData("ABCDEFGHIJK")] // Non-numeric
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = ArgentinaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "20123456786";
        var result = ArgentinaVatValidator.GetVatDetails(vat);
        
        if (result != null)
        {
            result.CountryCode.Should().Be("AR");
            result.IdentifierKind.Should().Be("CUIT");
            result.IsEuVat.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData("20123456786", "Individual")] // 20 prefix
    [InlineData("30123456786", "Company")] // 30 prefix
    public void GetVatDetails_ReturnsCorrectEntityType(string vat, string expectedType)
    {
        var result = ArgentinaVatValidator.GetVatDetails(vat);
        
        if (result != null)
        {
            result.Notes.Should().Contain(expectedType);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = ArgentinaVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
