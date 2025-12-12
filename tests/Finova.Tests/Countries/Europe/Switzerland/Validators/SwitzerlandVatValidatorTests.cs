using Finova.Countries.Europe.Switzerland.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Switzerland.Validators;

public class SwitzerlandVatValidatorTests
{
    [Theory]
    [InlineData("CHE107787577")]
    [InlineData("CH123456788")] // Assuming CH prefix is also supported by validator if implemented that way, check validator code if needed. EuropeVatValidator supports it.
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = SwitzerlandVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("CHE12345678")] // Too short
    [InlineData("CHE1234567890")] // Too long
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = SwitzerlandVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
