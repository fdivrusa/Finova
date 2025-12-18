using Finova.Countries.Europe.Norway.Validators;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Norway;

public class NorwayFodselsnummerValidatorTests
{
    private readonly NorwayFodselsnummerValidator _validator = new();

    [Theory]
    [InlineData("01010012356")]
    [InlineData("41010012269")] // D-number (Day + 40)
    public void Validate_ValidFodselsnummer_ReturnsSuccess(string id)
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
    [InlineData("0101001235")] // Too short
    [InlineData("010100123567")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("01010012357")] // Invalid Checksum 2
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }
}
