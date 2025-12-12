using Finova.Countries.Europe.Ukraine.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ukraine.Validators;

public class UkraineVatValidatorTests
{
    [Theory]
    [InlineData("09307222")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = UkraineVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("UA1234567")] // Too short
    [InlineData("UA1234567890")] // Too long
    [InlineData("XX123456789")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = UkraineVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
