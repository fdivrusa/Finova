using Finova.Countries.Europe.Luxembourg.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Luxembourg.Validators;

public class LuxembourgVatValidatorTests
{
    [Theory]
    [InlineData("LU15027442")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = LuxembourgVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("LU1234567")] // Too short
    [InlineData("LU123456789")] // Too long
    [InlineData("XX12345678")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = LuxembourgVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
