using Finova.Countries.Europe.Norway.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Norway.Validators;

public class NorwayVatValidatorTests
{
    [Theory]
    [InlineData("995567636")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = NorwayVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("NO12345678")] // Too short
    [InlineData("NO1234567890")] // Too long
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = NorwayVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
