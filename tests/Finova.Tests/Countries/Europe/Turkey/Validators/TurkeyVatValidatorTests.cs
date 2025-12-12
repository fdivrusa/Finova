using Finova.Countries.Europe.Turkey.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Turkey.Validators;

public class TurkeyVatValidatorTests
{
    [Theory]
    [InlineData("1000000000")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = TurkeyVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("TR123456789")] // Too short
    [InlineData("TR12345678901")] // Too long
    [InlineData("XX1234567890")] // Wrong prefix
    [InlineData("1000000001")] // Invalid checksum
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = TurkeyVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
