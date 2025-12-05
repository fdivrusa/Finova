using Finova.Countries.Europe.Denmark.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Denmark.Validators;

public class DenmarkIbanValidatorTests
{
    private readonly DenmarkIbanValidator _validator;

    public DenmarkIbanValidatorTests()
    {
        _validator = new DenmarkIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsDK()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("DK");
    }

    [Theory]
    [InlineData("DK5000400440116243")] // Danske Bank example
    public void IsValidIban_WithValidDanishIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("DK50 0040 0440 1162 43")] // With spaces
    [InlineData("dk5000400440116243")] // Lowercase
    [InlineData("Dk50 0040 0440 1162 43")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("DK0000400440116243")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("DK500040044011624")] // Too short (17 chars)
    [InlineData("DK50004004401162431")] // Too long (19 chars)
    [InlineData("DK50004004401162XX")] // Contains letters in account
    [InlineData("")] // Empty
    [InlineData("   ")] // Whitespace
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Static Method Tests

    [Theory]
    [InlineData("DK5000400440116243")]
    public void ValidateDenmarkIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = DenmarkIbanValidator.ValidateDenmarkIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("DK50 0040 0440 1162 43")] // With spaces
    [InlineData("dk5000400440116243")] // Lowercase
    public void ValidateDenmarkIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = DenmarkIbanValidator.ValidateDenmarkIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("DK0000400440116243")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("DK500040044011624")] // Too short
    [InlineData("DK50004004401162431")] // Too long
    [InlineData("DK50004004401162XX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateDenmarkIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = DenmarkIbanValidator.ValidateDenmarkIban(iban);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly18Characters_ReturnsTrue()
    {
        // Danish IBANs must be exactly 18 characters
        var iban = "DK5000400440116243"; // 18 chars

        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(18);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("DE89370400440532013000")] // German IBAN
    public void IsValidIban_WithOtherCountryIbans_ReturnsFalse(string iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_VerifiesAllDigitsAfterCountryCode()
    {
        // Danish IBANs must be all digits after DK
        var validIban = "DK5000400440116243";

        // Act
        var result = _validator.IsValidIban(validIban);

        // Assert
        result.Should().BeTrue();

        // Verify structure
        var normalized = validIban.ToUpper();
        normalized[2..18].All(char.IsDigit).Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "DK5000400440116243";

        // Act
        var result1 = _validator.IsValidIban(iban);
        var result2 = _validator.IsValidIban(iban);

        // Assert
        result1.Should().Be(result2);
    }

    #endregion
}
