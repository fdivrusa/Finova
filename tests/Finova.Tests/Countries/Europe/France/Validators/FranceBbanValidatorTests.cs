using Finova.Core.Common;
using Finova.Countries.Europe.France.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.France.Validators;

public class FranceBbanValidatorTests
{
    [Theory]
    [InlineData("30003020540004025069111")] // Valid
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = FranceBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = FranceBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("30003 02054 00040250691 00")] // Invalid Checksum
    [InlineData("ABC")]                        // Invalid Length
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = FranceBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
