using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Validators;

public class BelgiumNationalIdValidatorTests
{
    [Theory]
    [InlineData("72020290081")] // Valid (Born before 2000)
    [InlineData("01010100126")] // Valid (Born after 2000)
    [InlineData("72.02.02-900.81")] // Valid with separators
    public void Validate_ValidId_ReturnsSuccess(string input)
    {
        // Act
        var result = BelgiumNationalIdValidator.ValidateStatic(input);

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
        var result = BelgiumNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("123")] // Too short
    [InlineData("123456789012")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        // Act
        var result = BelgiumNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("72020290082")] // Invalid checksum
    [InlineData("01010100127")] // Invalid checksum (2000+)
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        // Act
        var result = BelgiumNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_InvalidFormat_ReturnsFailure()
    {
        // Act
        var result = BelgiumNationalIdValidator.ValidateStatic("ABCDEFGHIJK");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidFormat);
    }
}
