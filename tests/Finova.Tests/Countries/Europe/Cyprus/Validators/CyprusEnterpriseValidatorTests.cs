using Finova.Core.Common;
using Finova.Countries.Europe.Cyprus.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Cyprus.Validators;

public class CyprusEnterpriseValidatorTests
{
    [Theory]
    [InlineData("10259033P")] // Valid TIC
    [InlineData("CY10259033P")] // Valid TIC with prefix
    [InlineData("10259033 P")] // Valid TIC with space
    public void Validate_ValidTic_ReturnsSuccess(string? tic)
    {
        var result = CyprusTicValidator.ValidateTic(tic);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? tic)
    {
        var result = CyprusTicValidator.ValidateTic(tic);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.ErrorCode());
    }

    [Theory]
    [InlineData("1234567")] // Too short
    [InlineData("123456789")] // Too long
    [InlineData("ABCDEFGH")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string? tic)
    {
        var result = CyprusTicValidator.ValidateTic(tic);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.ErrorCode());
    }

    [Theory]
    [InlineData("12000138X")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string? tic)
    {
        var result = CyprusTicValidator.ValidateTic(tic);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidChecksum, result.ErrorCode());
    }
}
