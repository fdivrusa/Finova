using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.CzechRepublic.Validators;

public class CzechRepublicEnterpriseValidatorTests
{
    [Theory]
    [InlineData("26168685")] // Valid IČO (Seznam.cz)
    [InlineData("CZ26168685")] // Valid IČO with prefix
    [InlineData("26168685 ")] // Valid IČO with space
    [InlineData("25596641")] // Valid IČO (Another example)
    public void Validate_ValidIco_ReturnsSuccess(string? ico)
    {
        var result = CzechRepublicIcoValidator.ValidateIco(ico);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? ico)
    {
        var result = CzechRepublicIcoValidator.ValidateIco(ico);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.ErrorCode());
    }

    [Theory]
    [InlineData("1234567")] // Too short
    [InlineData("123456789")] // Too long
    [InlineData("ABCDEFGH")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string? ico)
    {
        var result = CzechRepublicIcoValidator.ValidateIco(ico);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.ErrorCode());
    }

    [Theory]
    [InlineData("12345678")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string? ico)
    {
        var result = CzechRepublicIcoValidator.ValidateIco(ico);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidChecksum, result.ErrorCode());
    }
}
