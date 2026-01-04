using Finova.Countries.SouthAmerica.Chile.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Chile;

public class ChileVatValidatorTests
{
    [Theory]
    [InlineData("12345678-5")] // Valid RUT with correct checksum
    [InlineData("CL123456785")] // With CL prefix
    [InlineData("12.345.678-5")] // Formatted
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = ChileVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1")] // Too short
    [InlineData("1234567890")] // Too long (10 chars)
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = ChileVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "12345678K";
        var result = ChileVatValidator.GetVatDetails(vat);

        if (result != null)
        {
            result.CountryCode.Should().Be("CL");
            result.IdentifierKind.Should().Be("RUT");
            result.IsEuVat.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = ChileVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
