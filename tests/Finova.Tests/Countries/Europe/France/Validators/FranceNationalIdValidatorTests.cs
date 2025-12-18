using Finova.Core.Common;
using Finova.Countries.Europe.France.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.France;

public class FranceNationalIdValidatorTests
{
    [Theory]
    [InlineData("1 80 01 45 000 000 69")] // Valid Male
    [InlineData("2 80 01 45 000 000 19")] // Valid Female
    [InlineData("180014500000069")]       // Valid No Spaces
    [InlineData("1 80 01 2A 000 000 92")] // Valid Corsica 2A
    [InlineData("1 80 01 2B 000 000 22")] // Valid Corsica 2B
    public void Validate_ValidId_ReturnsSuccess(string input)
    {
        // Act
        var result = FranceNationalIdValidator.ValidateStatic(input);

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
        var result = FranceNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("1800145000000")]    // Too short
    [InlineData("1800145000000979")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        // Act
        var result = FranceNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("180014500000098")] // Wrong Checksum (Should be 97)
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        // Act
        var result = FranceNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidChecksum);
    }

    [Theory]
    [InlineData("A80014500000097")] // Invalid char at start
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        // Act
        var result = FranceNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }
}
