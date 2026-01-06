using Finova.Core.Common;
using Finova.Countries.Europe.Denmark.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Denmark.Validators;

public class DenmarkEnterpriseValidatorTests
{
    [Theory]
    [InlineData("10121361")] // Valid CVR (LEGO)
    [InlineData("DK10121361")] // Valid CVR with prefix
    [InlineData("10121361 ")] // Valid CVR with space
    public void Validate_ValidCvr_ReturnsSuccess(string? cvr)
    {
        var result = DenmarkCvrValidator.ValidateCvr(cvr);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? cvr)
    {
        var result = DenmarkCvrValidator.ValidateCvr(cvr);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.ErrorCode());
    }

    [Theory]
    [InlineData("1234567")] // Too short
    [InlineData("123456789")] // Too long
    [InlineData("ABCDEFGH")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string? cvr)
    {
        var result = DenmarkCvrValidator.ValidateCvr(cvr);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.ErrorCode());
    }

    [Theory]
    [InlineData("88146320")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string? cvr)
    {
        var result = DenmarkCvrValidator.ValidateCvr(cvr);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidChecksum, result.ErrorCode());
    }
}
