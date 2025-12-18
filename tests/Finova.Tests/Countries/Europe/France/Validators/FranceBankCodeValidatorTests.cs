using Finova.Countries.Europe.France.Validators;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.France.Validators;

public class FranceBankCodeValidatorTests
{
    private readonly FranceBankCodeValidator _validator = new();

    [Theory]
    [InlineData("30003")]
    [InlineData("30004")]
    [InlineData("12345")]
    public void Validate_ValidBankCode_ReturnsSuccess(string code)
    {
        // Act
        var result = _validator.Validate(code);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("30 003")]
    [InlineData(" 12345 ")]
    public void Validate_ValidBankCodeWithSpaces_ReturnsSuccess(string code)
    {
        // Act
        var result = _validator.Validate(code);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? code)
    {
        // Act
        var result = _validator.Validate(code);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("1234")] // Too short
    [InlineData("123456")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string code)
    {
        // Act
        var result = _validator.Validate(code);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("1234A")] // Non-digit
    [InlineData("12-34")] // Invalid separator (only spaces allowed by implementation)
    public void Validate_InvalidFormat_ReturnsFailure(string code)
    {
        // Act
        var result = _validator.Validate(code);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("30003", "30003")]
    [InlineData("30 003", "30003")]
    public void Parse_ValidBankCode_ReturnsSanitizedInput(string input, string expected)
    {
        // Act
        var result = _validator.Parse(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1234")]
    public void Parse_InvalidBankCode_ReturnsNull(string? input)
    {
        // Act
        var result = _validator.Parse(input);

        // Assert
        result.Should().BeNull();
    }
}
