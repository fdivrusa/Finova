using Finova.Countries.Africa.SouthAfrica.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Africa.SouthAfrica;

public class SouthAfricaVatValidatorTests
{
    // Note: South African VAT numbers must start with 4 and have valid Luhn checksum
    // For testing, we use numbers that satisfy both conditions

    [Fact]
    public void Validate_WithValidFormat_But_InvalidChecksum_ReturnsFailure()
    {
        // 4000000000 has correct format but invalid Luhn
        var result = SouthAfricaVatValidator.Validate("4000000000");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("checksum", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456789")] // 9 digits - too short
    [InlineData("12345678901")] // 11 digits - too long
    [InlineData("5123456789")] // Starts with 5, not 4
    [InlineData("0123456789")] // Starts with 0
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = SouthAfricaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidChecksum_ReturnsFailure()
    {
        // 4111111111 - valid format, but likely invalid checksum
        var result = SouthAfricaVatValidator.Validate("4111111111");
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_WithInvalidVat_ReturnsNull()
    {
        // Invalid checksum should return null from GetVatDetails
        var result = SouthAfricaVatValidator.GetVatDetails("4000000000");
        result.Should().BeNull();
    }

    [Fact]
    public void Validate_ChecksFormatRequirements()
    {
        // Verify the format check (must start with 4)
        var result1 = SouthAfricaVatValidator.Validate("3123456789");
        result1.IsValid.Should().BeFalse();

        var result2 = SouthAfricaVatValidator.Validate("5123456789");
        result2.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("5123456789")] // Invalid starting digit
    public void GetVatDetails_WithInvalidInputs_ReturnsNull(string? vat)
    {
        var result = SouthAfricaVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
