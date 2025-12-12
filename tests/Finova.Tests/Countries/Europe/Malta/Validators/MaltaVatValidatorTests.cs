using Finova.Countries.Europe.Malta.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Malta.Validators;

public class MaltaVatValidatorTests
{
    [Theory]
    [InlineData("MT12345674")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = MaltaVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("MT1234567")] // Too short
    [InlineData("MT123456789")] // Too long
    [InlineData("XX12345678")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = MaltaVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
