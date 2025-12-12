using Finova.Countries.Europe.Iceland.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Iceland.Validators;

public class IcelandVatValidatorTests
{
    [Theory]
    [InlineData("1802695209")]
    [InlineData("IS12345")] // 5 digits is valid
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = IcelandVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("IS1234")] // Too short
    [InlineData("IS1234567")] // Too long
    [InlineData("XX123456")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = IcelandVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
