using Finova.Countries.Europe.Slovakia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovakia;

/// <summary>
/// Unit tests for the Slovakia IČO (Identifikačné číslo organizácie) validator.
/// </summary>
public class SlovakiaIcoValidatorTests
{
    private readonly SlovakiaIcoValidator _validator = new();

    #region Valid IČO Numbers

    [Theory]
    [InlineData("35780622")]      // Valid IČO with correct check digit
    [InlineData("31322832")]      // Another valid IČO
    [InlineData("36631124")]      // Valid IČO
    [InlineData("SK35780622")]    // With SK prefix
    [InlineData("sk35780622")]    // Lowercase SK prefix
    [InlineData(" 35780622 ")]    // With whitespace
    [InlineData("35-780-622")]    // With hyphens
    public void ValidateIco_ValidNumbers_ReturnsSuccess(string number)
    {
        // Act
        var result = SlovakiaIcoValidator.ValidateIco(number);

        // Assert
        result.IsValid.Should().BeTrue($"IČO '{number}' should be valid");
    }

    [Theory]
    [InlineData("35780622", "35780622")]
    [InlineData("SK35780622", "35780622")]
    [InlineData("sk35780622", "35780622")]
    [InlineData(" 35780622 ", "35780622")]
    [InlineData("35-780-622", "35780622")]
    public void Normalize_ValidNumbers_ReturnsExpected(string input, string expected)
    {
        // Act
        var result = SlovakiaIcoValidator.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Invalid IČO Numbers

    [Theory]
    [InlineData("")]              // Empty
    [InlineData("   ")]           // Whitespace only
    [InlineData("1234567")]       // Too short (7 digits)
    [InlineData("123456789")]     // Too long (9 digits)
    [InlineData("01234567")]      // Starts with 0
    [InlineData("ABCDEFGH")]      // Letters
    [InlineData("35780623")]      // Invalid check digit
    [InlineData("12345678")]      // Random number with wrong check digit
    public void ValidateIco_InvalidNumbers_ReturnsFailure(string number)
    {
        // Act
        var result = SlovakiaIcoValidator.ValidateIco(number);

        // Assert
        result.IsValid.Should().BeFalse($"IČO '{number}' should be invalid");
    }

    [Fact]
    public void ValidateIco_Null_ReturnsFailure()
    {
        // Act
        var result = SlovakiaIcoValidator.ValidateIco(null);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Check Digit Tests

    [Theory]
    [InlineData("35780622", true)]  // Valid check digit
    [InlineData("35780621", false)] // Invalid check digit
    [InlineData("35780623", false)] // Invalid check digit
    [InlineData("31322832", true)]  // Valid check digit
    [InlineData("31322831", false)] // Invalid check digit
    public void ValidateIco_CheckDigitValidation_ReturnsExpected(string number, bool expectedValid)
    {
        // Act
        var result = SlovakiaIcoValidator.ValidateIco(number);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }

    #endregion

    #region Parse Tests

    [Theory]
    [InlineData("35780622", "35780622")]
    [InlineData("SK35780622", "35780622")]
    [InlineData("", null)]
    [InlineData("invalid", null)]
    [InlineData("12345678", null)]  // Invalid check digit
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
    public void CountryCode_ReturnsSK()
    {
        // Assert
        _validator.CountryCode.Should().Be("SK");
    }

    [Fact]
    public void Validate_ValidNumber_ReturnsSuccess()
    {
        // Act
        var result = _validator.Validate("35780622");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}
