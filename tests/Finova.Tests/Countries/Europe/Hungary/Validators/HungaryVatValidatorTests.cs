using Finova.Countries.Europe.Hungary.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Hungary.Validators;

public class HungaryVatValidatorTests
{
    [Theory]
    [InlineData("HU12892312")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = HungaryVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("HU1234567")] // Too short
    [InlineData("HU123456789")] // Too long
    [InlineData("XX12345678")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = HungaryVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
