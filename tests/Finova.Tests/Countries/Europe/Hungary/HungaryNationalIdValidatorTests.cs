using Finova.Countries.Europe.Hungary.Validators;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Hungary;

public class HungaryNationalIdValidatorTests
{
    private readonly HungaryNationalIdValidator _validator = new();

    [Theory]
    [InlineData("123456788")] // Calculated valid
    [InlineData("123-456-788")] // Valid with dashes
    public void Validate_ValidTaj_ReturnsSuccess(string id)
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
    [InlineData("12345678")] // Too short
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("123456789")] // Invalid Checksum (Should be 8)
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }
}
