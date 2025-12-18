using Finova.Core.Common;
using Finova.Countries.Europe.Italy.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Italy.Validators;

public class ItalyBbanValidatorTests
{
    [Theory]
    [InlineData("X0542811101000000123456")] // Valid
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = ItalyBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = ItalyBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("Z 03002 03200 000000000200")] // Invalid Checksum (Control char mismatch)
    [InlineData("ABC")]                        // Invalid Length
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = ItalyBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
