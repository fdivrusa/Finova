using Finova.Countries.Europe.NorthMacedonia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.NorthMacedonia.Validators;

public class NorthMacedoniaVatValidatorTests
{
    [Theory]
    [InlineData("4030992255006")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = NorthMacedoniaVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("MK123456789012")] // Too short
    [InlineData("MK12345678901234")] // Too long
    [InlineData("XX1234567890123")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = NorthMacedoniaVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
