using Finova.Countries.Europe.Azerbaijan.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Azerbaijan.Validators;

public class AzerbaijanVatValidatorTests
{
    [Theory]
    [InlineData("AZ1234567890")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = AzerbaijanVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("AZ123456789")] // Too short
    [InlineData("AZ12345678901")] // Too long
    [InlineData("XX1234567890")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = AzerbaijanVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
