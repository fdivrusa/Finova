using Finova.Core.Common;
using Finova.Countries.Europe.Germany.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany.Validators;

public class GermanyBbanValidatorTests
{
    [Theory]
    [InlineData("370400440532013000")] // Valid
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = GermanyBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = GermanyBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                    // Invalid Length - too short
    [InlineData("37040044053201300")]      // Invalid Length - 17 digits (missing one)
    [InlineData("3704004405320130000")]    // Invalid Length - 19 digits (one extra)
    [InlineData("3704004405320130A0")]     // Invalid Format - contains letter
    [InlineData("370400X40532013000")]     // Invalid Format - contains letter in middle
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = GermanyBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
