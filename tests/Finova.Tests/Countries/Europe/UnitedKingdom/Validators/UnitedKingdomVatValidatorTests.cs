using Finova.Countries.Europe.UnitedKingdom.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.UnitedKingdom.Validators;

public class UnitedKingdomVatValidatorTests
{
    [Theory]
    [InlineData("GB434031494")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = UnitedKingdomVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("GB12345678")] // Too short (standard 9, but UK has other formats too. 123456789 is standard)
    [InlineData("GB1234567890123")] // Too long (standard 9 or 12, branch traders 12)
    // GB regex: ^\d{9}$|^\d{12}$|^GD\d{3}$|^HA\d{3}$
    // So 12 digits is valid. Let's test 12 digits as valid too?
    // Or just test standard 9.
    // Let's stick to standard 9 for success.
    // Invalid:
    [InlineData("GB1234567")] // Too short
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = UnitedKingdomVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
