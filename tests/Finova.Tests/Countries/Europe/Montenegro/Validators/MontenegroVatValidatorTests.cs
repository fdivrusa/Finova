using Finova.Countries.Europe.Montenegro.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Montenegro.Validators;

public class MontenegroVatValidatorTests
{
    [Theory]
    [InlineData("10000004")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = MontenegroVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ME1234567")] // Too short
    [InlineData("ME123456789")] // Too long
    [InlineData("XX12345678")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = MontenegroVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
