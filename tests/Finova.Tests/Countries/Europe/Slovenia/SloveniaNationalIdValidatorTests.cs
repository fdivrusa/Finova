using Finova.Countries.Europe.Slovenia.Validators;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovenia;

public class SloveniaNationalIdValidatorTests
{
    private readonly SloveniaNationalIdValidator _validator = new();

    [Theory]
    [InlineData("0101980500005")] // Calculated valid
    public void Validate_ValidEmso_ReturnsSuccess(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("010198050000")] // Too short
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("0101980500006")] // Invalid Checksum (Should be 5)
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }
}
