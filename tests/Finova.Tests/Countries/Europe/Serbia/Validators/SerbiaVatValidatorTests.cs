using Finova.Countries.Europe.Serbia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Serbia.Validators;

public class SerbiaVatValidatorTests
{
    [Theory]
    [InlineData("RS100000024")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = SerbiaVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("RS12345678")] // Too short
    [InlineData("RS1234567890")] // Too long
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = SerbiaVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
