using Finova.Countries.Europe.Moldova.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Moldova.Validators;

public class MoldovaVatValidatorTests
{
    [Theory]
    [InlineData("MD1234567890123")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = MoldovaVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("MD123456789012")] // Too short
    [InlineData("MD12345678901234")] // Too long
    [InlineData("XX1234567890123")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = MoldovaVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
