using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Countries.Europe.Estonia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Estonia.Validators;

public class EstoniaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("10000356")] // Valid Registrikood
    [InlineData("EE10000356")] // Valid Registrikood with prefix
    [InlineData("10000356 ")] // Valid Registrikood with space
    public void Validate_ValidRegistrikood_ReturnsSuccess(string? registrikood)
    {
        var result = EstoniaRegistrikoodValidator.ValidateRegistrikood(registrikood);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? registrikood)
    {
        var result = EstoniaRegistrikoodValidator.ValidateRegistrikood(registrikood);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.ErrorCode());
    }

    [Theory]
    [InlineData("1234567")] // Too short
    [InlineData("123456789")] // Too long
    [InlineData("ABCDEFGH")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string? registrikood)
    {
        var result = EstoniaRegistrikoodValidator.ValidateRegistrikood(registrikood);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.ErrorCode());
    }

    [Theory]
    [InlineData("10224771")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string? registrikood)
    {
        var result = EstoniaRegistrikoodValidator.ValidateRegistrikood(registrikood);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidChecksum, result.ErrorCode());
    }
}
