using Finova.Countries.Europe.Malta.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Malta;

/// <summary>
/// Unit tests for the Malta Company Registration Number validator.
/// </summary>
public class MaltaCompanyNumberValidatorTests
{
    private readonly MaltaCompanyNumberValidator _validator = new();

    #region Valid Company Numbers

    [Theory]
    [InlineData("C12345")]        // Standard C prefix + 5 digits
    [InlineData("C123456")]       // C prefix + 6 digits
    [InlineData("P12345")]        // Partnership prefix
    [InlineData("F12345")]        // Foundation prefix
    [InlineData("O12345")]        // Other prefix
    [InlineData("E12345")]        // Economic association prefix
    [InlineData("c12345")]        // Lowercase prefix
    [InlineData("MT C12345")]     // With MT prefix
    [InlineData("mtc12345")]      // Lowercase MT prefix
    [InlineData(" C12345 ")]      // With whitespace
    [InlineData("C-12345")]       // With hyphen
    public void ValidateCompanyNumber_ValidNumbers_ReturnsSuccess(string number)
    {
        // Act
        var result = MaltaCompanyNumberValidator.ValidateCompanyNumber(number);

        // Assert
        result.IsValid.Should().BeTrue($"Company number '{number}' should be valid");
    }

    [Theory]
    [InlineData("C12345", "C12345")]
    [InlineData("c12345", "C12345")]
    [InlineData("MT C12345", "C12345")]
    [InlineData("mtc12345", "C12345")]
    [InlineData(" C12345 ", "C12345")]
    [InlineData("C-12345", "C12345")]
    public void Normalize_ValidNumbers_ReturnsExpected(string input, string expected)
    {
        // Act
        var result = MaltaCompanyNumberValidator.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Invalid Company Numbers

    [Theory]
    [InlineData("")]              // Empty
    [InlineData("   ")]           // Whitespace only
    [InlineData("12345")]         // No prefix
    [InlineData("C1234")]         // Too few digits (4)
    [InlineData("C1234567")]      // Too many digits (7)
    [InlineData("X12345")]        // Invalid prefix
    [InlineData("CC12345")]       // Double prefix
    [InlineData("CABCDE")]        // Letters instead of digits
    [InlineData("12345C")]        // Prefix at end
    public void ValidateCompanyNumber_InvalidNumbers_ReturnsFailure(string number)
    {
        // Act
        var result = MaltaCompanyNumberValidator.ValidateCompanyNumber(number);

        // Assert
        result.IsValid.Should().BeFalse($"Company number '{number}' should be invalid");
    }

    [Fact]
    public void ValidateCompanyNumber_Null_ReturnsFailure()
    {
        // Act
        var result = MaltaCompanyNumberValidator.ValidateCompanyNumber(null);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Parse Tests

    [Theory]
    [InlineData("C12345", "C12345")]
    [InlineData("P12345", "P12345")]
    [InlineData("MT C12345", "C12345")]
    [InlineData("", null)]
    [InlineData("invalid", null)]
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
    public void CountryCode_ReturnsMT()
    {
        // Assert
        _validator.CountryCode.Should().Be("MT");
    }

    [Fact]
    public void Validate_ValidNumber_ReturnsSuccess()
    {
        // Act
        var result = _validator.Validate("C12345");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}
