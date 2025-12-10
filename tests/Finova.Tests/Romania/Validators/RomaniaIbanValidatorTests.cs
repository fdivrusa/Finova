using Finova.Countries.Europe.Romania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Romania.Validators;

public class RomaniaIbanValidatorTests
{
    private readonly RomaniaIbanValidator _validator;

    public RomaniaIbanValidatorTests()
    {
        _validator = new RomaniaIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsRO()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("RO");
    }

    [Theory]
    [InlineData("RO49AAAA1B31007593840000")] // BCR example with letters
    [InlineData("RO66BACX0000001234567890")] // UniCredit example
    [InlineData("RO09BCYP0000001234567890")] // OTP Bank example
    public void IsValidIban_WithValidRomanianIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RO49 AAAA 1B31 0075 9384 0000")] // With spaces
    [InlineData("ro49aaaa1b31007593840000")] // Lowercase
    [InlineData("Ro49 AAAA 1B31 0075 9384 0000")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RO00AAAA1B31007593840000")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("RO49AAAA1B3100759384000")] // Too short (23 chars)
    [InlineData("RO49AAAA1B310075938400001")] // Too long (25 chars)
    [InlineData("")] // Empty
    [InlineData("   ")] // Whitespace
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Static Method Tests

    [Theory]
    [InlineData("RO49AAAA1B31007593840000")]
    [InlineData("RO66BACX0000001234567890")]
    [InlineData("RO09BCYP0000001234567890")]
    public void ValidateRomaniaIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = RomaniaIbanValidator.ValidateRomaniaIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RO49 AAAA 1B31 0075 9384 0000")] // With spaces
    [InlineData("ro49aaaa1b31007593840000")] // Lowercase
    public void ValidateRomaniaIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = RomaniaIbanValidator.ValidateRomaniaIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RO00AAAA1B31007593840000")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("RO49AAAA1B3100759384000")] // Too short
    [InlineData("RO49AAAA1B310075938400001")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateRomaniaIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = RomaniaIbanValidator.ValidateRomaniaIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly24Characters_ReturnsTrue()
    {
        // Romanian IBANs must be exactly 24 characters
        var iban = "RO49AAAA1B31007593840000"; // 24 chars

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(24);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("DE89370400440532013000")] // German IBAN
    public void IsValidIban_WithOtherCountryIbans_ReturnsFalse(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "RO49AAAA1B31007593840000";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion

    #region Romanian-Specific Alphanumeric Account Tests

    [Theory]
    [InlineData("RO49AAAA1B31007593840000")] // Letters in bank code and account
    [InlineData("RO66BACX0000001234567890")] // Letters in bank code
    [InlineData("RO09BCYP0000001234567890")] // Letters in bank code
    public void IsValidIban_WithAlphanumericBankCodeAndAccount_ReturnsTrue(string iban)
    {
        // Romanian IBANs allow letters in bank code (4 chars) and account (16 chars)
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithOnlyDigitsInAccount_ReturnsTrue()
    {
        // Romanian IBANs with only digits in account should also be valid
        var iban = "RO66BACX0000001234567890";

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithLettersInAccount_ReturnsTrue()
    {
        // Romanian IBANs with letters in account should also be valid
        var iban = "RO49AAAA1B31007593840000";

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();

        // Verify structure - Bank code (4 chars) should be letters, account can be alphanumeric
        var normalized = iban.ToUpper();
        normalized.Substring(4, 4).All(char.IsLetter).Should().BeTrue();
    }

    #endregion
}

