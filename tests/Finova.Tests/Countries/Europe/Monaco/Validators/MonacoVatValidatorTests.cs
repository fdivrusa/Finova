using Finova.Countries.Europe.Monaco.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Monaco.Validators;

public class MonacoVatValidatorTests
{
    [Theory]
    [InlineData("MC44732829320")]
    [InlineData("FR44732829320")]
    [InlineData("44732829320")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = MonacoVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("MC1234567890")] // Too short
    [InlineData("MC123456789012")] // Too long
    [InlineData("XX12345678901")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = MonacoVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
