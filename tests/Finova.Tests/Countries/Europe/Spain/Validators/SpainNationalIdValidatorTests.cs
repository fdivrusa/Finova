using Finova.Countries.Europe.Spain.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Spain.Validators;

public class SpainNationalIdValidatorTests
{
    [Theory]
    [InlineData("12345678Z")] // Valid DNI
    [InlineData("X1234567L")] // Valid NIE (X)
    [InlineData("Y1234567X")] // Valid NIE (Y)
    [InlineData("Z1234567R")] // Valid NIE (Z)
    [InlineData("12345678-Z")] // Valid with separator
    [InlineData("X-1234567-L")] // Valid with separators
    public void Validate_ValidId_ReturnsSuccess(string input)
    {
        // Act
        var result = SpainNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        // Act
        var result = SpainNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("1234567Z")] // Too short
    [InlineData("123456789Z")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        // Act
        var result = SpainNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("A1234567L")] // Invalid prefix
    [InlineData("123456789")] // Missing letter
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        // Act
        var result = SpainNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("12345678A")] // Invalid checksum (Should be Z)
    [InlineData("X1234567A")] // Invalid checksum (Should be L)
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        // Act
        var result = SpainNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidChecksum);
    }
}
