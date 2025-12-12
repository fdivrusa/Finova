using Finova.Countries.Europe.Greece.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Greece.Validators;

public class GreeceVatValidatorTests
{
    [Theory]
    [InlineData("EL123456783")]
    [InlineData("GR123456783")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = GreeceVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("EL12345678")] // Too short
    [InlineData("EL1234567890")] // Too long
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = GreeceVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
