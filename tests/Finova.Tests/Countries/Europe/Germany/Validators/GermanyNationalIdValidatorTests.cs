using Finova.Countries.Europe.Germany.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany.Validators;

public class GermanyNationalIdValidatorTests
{
    [Theory]
    [InlineData("T22000124")] // Calculated valid
    [InlineData("L01X00T44")] // Calculated valid
    [InlineData("T2200012 4")] // With space
    [InlineData("L01X00T4-4")] // With dash
    public void Validate_ValidId_ReturnsSuccess(string input)
    {
        // Act
        var result = GermanyNationalIdValidator.ValidateStatic(input);

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
        var result = GermanyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("T2200012")] // Too short
    [InlineData("T2200012933")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        // Act
        var result = GermanyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("T22000125")] // Wrong checksum (last digit should be 4)
    [InlineData("L01X00T48")] // Wrong checksum (last digit should be 4)
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        // Act
        var result = GermanyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidChecksum);
    }
}
