using Finova.Core.Common;
using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Validators;

public class BelgiumBbanValidatorTests
{
    [Theory]
    [InlineData("000000001919")] // Valid (19 % 97 = 19)
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = BelgiumBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = BelgiumBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("000000001997")] // Invalid Checksum (19 != 97)
    [InlineData("ABC")]          // Invalid Length
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = BelgiumBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_ShouldReturnSanitizedString_WhenValid()
    {
        // Arrange
        var validator = new BelgiumBbanValidator();
        var input = "000-0000019-19"; // Valid with separators

        // Act
        var result = validator.Parse(input);

        // Assert
        result.Should().Be("000000001919");
    }

    [Fact]
    public void Parse_ShouldReturnNull_WhenInvalid()
    {
        // Arrange
        var validator = new BelgiumBbanValidator();
        var input = "000-0000019-99"; // Invalid checksum

        // Act
        var result = validator.Parse(input);

        // Assert
        result.Should().BeNull();
    }
}
