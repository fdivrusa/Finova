using Finova.Countries.Europe.Ireland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ireland;

/// <summary>
/// Unit tests for the Irish CRO (Companies Registration Office) number validator.
/// </summary>
public class IrelandCroValidatorTests
{
    private readonly IrelandCroValidator _validator = new();

    #region Valid CRO Numbers

    [Theory]
    [InlineData("123456")]        // Standard 6-digit format
    [InlineData("A12345")]        // Letter prefix + 5 digits
    [InlineData("1")]             // Single digit
    [InlineData("12")]            // Two digits
    [InlineData("123")]           // Three digits
    [InlineData("1234")]          // Four digits
    [InlineData("12345")]         // Five digits
    [InlineData("B1")]            // Letter prefix + 1 digit
    [InlineData("C123")]          // Letter prefix + 3 digits
    [InlineData("IE123456")]      // With IE prefix
    [InlineData("ie123456")]      // Lowercase IE prefix
    [InlineData("IE A12345")]     // With IE prefix and space
    [InlineData(" 123456 ")]      // With whitespace
    [InlineData("123-456")]       // With hyphen
    public void ValidateCro_ValidNumbers_ReturnsSuccess(string number)
    {
        // Act
        var result = IrelandCroValidator.ValidateCro(number);

        // Assert
        result.IsValid.Should().BeTrue($"CRO number '{number}' should be valid");
    }

    [Theory]
    [InlineData("123456", "123456")]
    [InlineData("A12345", "A12345")]
    [InlineData("ie123456", "123456")]
    [InlineData("IE123456", "123456")]
    [InlineData("IE A12345", "A12345")]
    [InlineData(" 123456 ", "123456")]
    [InlineData("123-456", "123456")]
    public void Normalize_ValidNumbers_ReturnsExpected(string input, string expected)
    {
        // Act
        var result = IrelandCroValidator.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Invalid CRO Numbers

    [Theory]
    [InlineData("")]              // Empty
    [InlineData("   ")]           // Whitespace only
    [InlineData("1234567")]       // Too many digits (7)
    [InlineData("A1234567")]      // Letter prefix + too many digits
    [InlineData("AB12345")]       // Double letter prefix
    [InlineData("12AB34")]        // Letters in middle
    [InlineData("ABCDEF")]        // Letters only
    public void ValidateCro_InvalidNumbers_ReturnsFailure(string number)
    {
        // Act
        var result = IrelandCroValidator.ValidateCro(number);

        // Assert
        result.IsValid.Should().BeFalse($"CRO number '{number}' should be invalid");
    }

    [Fact]
    public void ValidateCro_Null_ReturnsFailure()
    {
        // Act
        var result = IrelandCroValidator.ValidateCro(null);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Parse Tests

    [Theory]
    [InlineData("123456", "123456")]
    [InlineData("A12345", "A12345")]
    [InlineData("IE123456", "123456")]
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
    public void CountryCode_ReturnsIE()
    {
        // Assert
        _validator.CountryCode.Should().Be("IE");
    }

    [Fact]
    public void Validate_ValidNumber_ReturnsSuccess()
    {
        // Act
        var result = _validator.Validate("123456");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}
