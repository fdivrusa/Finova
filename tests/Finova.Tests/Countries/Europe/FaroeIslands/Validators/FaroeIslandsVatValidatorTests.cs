using Finova.Countries.Europe.FaroeIslands.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.FaroeIslands.Validators;

public class FaroeIslandsVatValidatorTests
{
    [Theory]
    [InlineData("FO123456")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = FaroeIslandsVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("FO12345")] // Too short
    [InlineData("FO1234567")] // Too long
    [InlineData("XX123456")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = FaroeIslandsVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
