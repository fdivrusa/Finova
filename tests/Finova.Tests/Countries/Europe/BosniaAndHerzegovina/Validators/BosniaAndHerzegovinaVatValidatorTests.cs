using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.BosniaAndHerzegovina.Validators;

public class BosniaAndHerzegovinaVatValidatorTests
{
    [Theory]
    [InlineData("4000000000005")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = BosniaAndHerzegovinaVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("BA123456789012")] // Too short
    [InlineData("BA12345678901234")] // Too long
    [InlineData("XX1234567890123")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = BosniaAndHerzegovinaVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
