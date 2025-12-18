using Finova.Core.Common;
using Finova.Countries.Europe.Spain.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Spain.Validators;

public class SpainBbanValidatorTests
{
    [Theory]
    [InlineData("00000000000000000000")] // Valid (All zeros is valid for Mod 11)
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = SpainBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = SpainBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("0182 5500 00 0201005001")] // Invalid Checksum
    [InlineData("ABC")]                     // Invalid Length
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = SpainBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
