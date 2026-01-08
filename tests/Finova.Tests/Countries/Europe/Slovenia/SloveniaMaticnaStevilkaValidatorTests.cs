using Finova.Countries.Europe.Slovenia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovenia;

/// <summary>
/// Unit tests for the Slovenia Matična številka (Registration Number) validator.
/// </summary>
public class SloveniaMaticnaStevilkaValidatorTests
{
    private readonly SloveniaMaticnaStevilkaValidator _validator = new();

    #region Valid Matična številka Numbers

    [Theory]
    [InlineData("1234567400")]    // Valid: Sum(1×7+2×6+3×5+4×4+5×3+6×2+7×1)=84, 84%11=7, check=4
    [InlineData("5300001100")]    // Valid: Sum(5×7+3×6+0×5+0×4+0×3+0×2+1×1)=54, 54%11=10, check=1
    [InlineData("1111111500")]    // Valid: Sum(1×7+1×6+1×5+1×4+1×3+1×2+1×1)=28, 28%11=6, check=5
    [InlineData("SI1234567400")]  // With SI prefix
    [InlineData("si1234567400")]  // Lowercase SI prefix
    [InlineData(" 1234567400 ")]  // With whitespace
    [InlineData("1234-567-400")]  // With hyphens
    public void ValidateMaticnaStevilka_ValidNumbers_ReturnsSuccess(string number)
    {
        // Act
        var result = SloveniaMaticnaStevilkaValidator.ValidateMaticnaStevilka(number);

        // Assert
        result.IsValid.Should().BeTrue($"Matična številka '{number}' should be valid");
    }

    [Theory]
    [InlineData("1234567400", "1234567400")]
    [InlineData("SI1234567400", "1234567400")]
    [InlineData("si1234567400", "1234567400")]
    [InlineData(" 1234567400 ", "1234567400")]
    [InlineData("1234-567-400", "1234567400")]
    public void Normalize_ValidNumbers_ReturnsExpected(string input, string expected)
    {
        // Act
        var result = SloveniaMaticnaStevilkaValidator.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Invalid Matična številka Numbers

    [Theory]
    [InlineData("")]              // Empty
    [InlineData("   ")]           // Whitespace only
    [InlineData("123456789")]     // Too short (9 digits)
    [InlineData("12345678901")]   // Too long (11 digits)
    [InlineData("0123456789")]    // Starts with 0
    [InlineData("ABCDEFGHIJ")]    // Letters
    [InlineData("1234567301")]    // Invalid check digit (should be 4, not 3)
    [InlineData("1234567891")]    // Random number with wrong check digit
    public void ValidateMaticnaStevilka_InvalidNumbers_ReturnsFailure(string number)
    {
        // Act
        var result = SloveniaMaticnaStevilkaValidator.ValidateMaticnaStevilka(number);

        // Assert
        result.IsValid.Should().BeFalse($"Matična številka '{number}' should be invalid");
    }

    [Fact]
    public void ValidateMaticnaStevilka_Null_ReturnsFailure()
    {
        // Act
        var result = SloveniaMaticnaStevilkaValidator.ValidateMaticnaStevilka(null);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Check Digit Tests

    [Theory]
    [InlineData("1234567400", true)]  // Valid check digit (4)
    [InlineData("1234567300", false)] // Invalid check digit (3 instead of 4)
    [InlineData("1234567500", false)] // Invalid check digit (5 instead of 4)
    public void ValidateMaticnaStevilka_CheckDigitValidation_ReturnsExpected(string number, bool expectedValid)
    {
        // Act
        var result = SloveniaMaticnaStevilkaValidator.ValidateMaticnaStevilka(number);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }

    #endregion

    #region Parse Tests

    [Theory]
    [InlineData("1234567400", "1234567400")]
    [InlineData("SI1234567400", "1234567400")]
    [InlineData("", null)]
    [InlineData("invalid", null)]
    [InlineData("1234567300", null)]  // Invalid check digit (3 instead of 4)
    public void Parse_Various_ReturnsExpected(string? input, string? expected)
    {
        // Act
        var result = _validator.Parse(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Interface Tests

    [Fact]
    public void CountryCode_ReturnsSI()
    {
        // Assert
        _validator.CountryCode.Should().Be("SI");
    }

    [Fact]
    public void Validate_ValidNumber_ReturnsSuccess()
    {
        // Act
        var result = _validator.Validate("1234567400");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}
