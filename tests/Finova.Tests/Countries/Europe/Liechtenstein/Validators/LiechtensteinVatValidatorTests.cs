using Finova.Countries.Europe.Liechtenstein.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Liechtenstein.Validators;

public class LiechtensteinVatValidatorTests
{
    [Theory]
    [InlineData("123456788")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = LiechtensteinVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("LI1234")] // Too short
    [InlineData("LI123456")] // Too long
    [InlineData("XX12345")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = LiechtensteinVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
