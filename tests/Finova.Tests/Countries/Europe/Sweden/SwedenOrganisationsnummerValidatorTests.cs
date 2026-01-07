using Finova.Countries.Europe.Sweden.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Sweden;

/// <summary>
/// Unit tests for the Swedish Organisationsnummer validator.
/// </summary>
public class SwedenOrganisationsnummerValidatorTests
{
    private readonly SwedenOrganisationsnummerValidator _validator = new();

    #region Valid Organisationsnummer

    [Theory]
    [InlineData("5560360793")]      // IKEA - valid Luhn
    [InlineData("556036-0793")]     // With hyphen
    [InlineData("2120000142")]      // Bolagsverket (Swedish Companies Registration Office)
    [InlineData("212000-0142")]     // Bolagsverket with hyphen
    [InlineData("5561040436")]      // H&M - valid Luhn
    [InlineData("SE5560360793")]    // With SE prefix
    [InlineData("se5560360793")]    // Lowercase SE prefix
    [InlineData(" 5560360793 ")]    // With whitespace
    [InlineData("556036 0793")]     // With space
    public void ValidateOrganisationsnummer_ValidNumbers_ReturnsSuccess(string number)
    {
        // Act
        var result = SwedenOrganisationsnummerValidator.ValidateOrganisationsnummer(number);

        // Assert
        result.IsValid.Should().BeTrue($"Organisationsnummer '{number}' should be valid");
    }

    [Theory]
    [InlineData("5560360793", "5560360793")]
    [InlineData("556036-0793", "5560360793")]
    [InlineData("SE5560360793", "5560360793")]
    [InlineData("se5560360793", "5560360793")]
    [InlineData(" 5560360793 ", "5560360793")]
    [InlineData("556036 0793", "5560360793")]
    public void Normalize_ValidNumbers_ReturnsExpected(string input, string expected)
    {
        // Act
        var result = SwedenOrganisationsnummerValidator.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("5560360793", "556036-0793")]
    [InlineData("556036-0793", "556036-0793")]
    [InlineData("SE5560360793", "556036-0793")]
    public void Format_ValidNumbers_ReturnsFormatted(string input, string expected)
    {
        // Act
        var result = SwedenOrganisationsnummerValidator.Format(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Invalid Organisationsnummer

    [Theory]
    [InlineData("")]                // Empty
    [InlineData("   ")]             // Whitespace only
    [InlineData("123456789")]       // Too short (9 digits)
    [InlineData("12345678901")]     // Too long (11 digits)
    [InlineData("0560360793")]      // First two digits < 10
    [InlineData("5560360794")]      // Invalid Luhn check digit
    [InlineData("ABCDEFGHIJ")]      // Letters
    [InlineData("5510360793")]      // Third digit < 2 for company (not individual)
    public void ValidateOrganisationsnummer_InvalidNumbers_ReturnsFailure(string number)
    {
        // Act
        var result = SwedenOrganisationsnummerValidator.ValidateOrganisationsnummer(number);

        // Assert
        result.IsValid.Should().BeFalse($"Organisationsnummer '{number}' should be invalid");
    }

    [Fact]
    public void ValidateOrganisationsnummer_Null_ReturnsFailure()
    {
        // Act
        var result = SwedenOrganisationsnummerValidator.ValidateOrganisationsnummer(null);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Luhn Check Digit Tests

    [Theory]
    [InlineData("5560360793", true)]  // Valid Luhn
    [InlineData("5560360794", false)] // Invalid Luhn
    [InlineData("5560360792", false)] // Invalid Luhn
    [InlineData("2120000142", true)]  // Valid Luhn (Bolagsverket)
    [InlineData("2120000143", false)] // Invalid Luhn
    public void ValidateOrganisationsnummer_LuhnValidation_ReturnsExpected(string number, bool expectedValid)
    {
        // Act
        var result = SwedenOrganisationsnummerValidator.ValidateOrganisationsnummer(number);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }

    #endregion

    #region Parse Tests

    [Theory]
    [InlineData("5560360793", "5560360793")]
    [InlineData("556036-0793", "5560360793")]
    [InlineData("SE5560360793", "5560360793")]
    [InlineData("", null)]
    [InlineData("invalid", null)]
    [InlineData("5560360794", null)]  // Invalid Luhn
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
    public void CountryCode_ReturnsSE()
    {
        // Assert
        _validator.CountryCode.Should().Be("SE");
    }

    [Fact]
    public void Validate_ValidNumber_ReturnsSuccess()
    {
        // Act
        var result = _validator.Validate("5560360793");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Real World Examples

    [Theory]
    [InlineData("5560360793")]      // IKEA
    [InlineData("5561040436")]      // H&M
    public void ValidateOrganisationsnummer_RealWorldExamples_ReturnsSuccess(string number)
    {
        // Act
        var result = SwedenOrganisationsnummerValidator.ValidateOrganisationsnummer(number);

        // Assert
        result.IsValid.Should().BeTrue($"Real-world organisationsnummer '{number}' should be valid");
    }

    #endregion
}
