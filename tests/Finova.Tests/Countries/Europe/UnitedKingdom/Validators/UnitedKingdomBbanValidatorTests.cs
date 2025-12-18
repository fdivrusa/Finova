using Finova.Core.Common;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.UnitedKingdom.Validators;

public class UnitedKingdomBbanValidatorTests
{
    [Theory]
    [InlineData("MIDL12345612345678")] // Valid
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = UnitedKingdomBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = UnitedKingdomBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("123456 1234567")] // Invalid Length
    [InlineData("ABC")]            // Invalid Format
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = UnitedKingdomBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
