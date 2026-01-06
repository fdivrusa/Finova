using Finova.Core.Common;
using Finova.Countries.Europe.Croatia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Croatia.Validators;

public class CroatiaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("94577403194")] // Valid OIB
    [InlineData("HR94577403194")] // Valid OIB with prefix
    [InlineData("94577403194 ")] // Valid OIB with space
    public void Validate_ValidOib_ReturnsSuccess(string? oib)
    {
        var result = CroatiaOibValidator.ValidateOib(oib);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? oib)
    {
        var result = CroatiaOibValidator.ValidateOib(oib);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.ErrorCode());
    }

    [Theory]
    [InlineData("1234567890")] // Too short
    [InlineData("123456789012")] // Too long
    [InlineData("ABCDEFGHIJK")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string? oib)
    {
        var result = CroatiaOibValidator.ValidateOib(oib);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.ErrorCode());
    }

    [Theory]
    [InlineData("69435151518")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string? oib)
    {
        var result = CroatiaOibValidator.ValidateOib(oib);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidChecksum, result.ErrorCode());
    }
}
