using Finova.Countries.Europe.Georgia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Georgia.Validators;

public class GeorgiaVatValidatorTests
{
    [Theory]
    [InlineData("GE123456789")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = GeorgiaVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("GE12345678")] // Too short
    [InlineData("GE1234567890")] // Too long
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = GeorgiaVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
