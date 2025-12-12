using Finova.Countries.Europe.CzechRepublic.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.CzechRepublic.Validators;

public class CzechRepublicVatValidatorTests
{
    [Theory]
    [InlineData("CZ25123891")]
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = CzechRepublicVatValidator.Validate(vat);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("CZ1234567")] // Too short
    [InlineData("CZ12345678901")] // Too long (assuming max 10 for now, but regex might be specific)
    [InlineData("XX12345678")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = CzechRepublicVatValidator.Validate(vat);
        Assert.False(result.IsValid);
    }
}
