using Finova.Countries.Europe.Ireland.Validators;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ireland;

public class IrelandNationalIdValidatorTests
{
    private readonly IrelandNationalIdValidator _validator = new();

    [Theory]
    [InlineData("1234567T")] // Valid 8 chars
    [InlineData("1234567FA")] // Valid 9 chars
    public void Validate_ValidPpsn_ReturnsSuccess(string id)
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
    [InlineData("123456")] // Too short
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("1234567A")] // Invalid Checksum (Should be T)
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }
}
