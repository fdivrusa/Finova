using Finova.Countries.Europe.Belarus.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belarus.Validators;

public class BelarusVatValidatorTests
{
    [Theory]
    [InlineData("BY100000007")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = BelarusVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("BY12345678")] // Too short
    [InlineData("BY1234567890")] // Too long
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = BelarusVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
