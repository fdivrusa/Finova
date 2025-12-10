using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Belgium.Validators;

public class BelgianBankAccountValidatorTests
{
    private readonly BelgiumIbanValidator _validator;

    public BelgianBankAccountValidatorTests()
    {
        _validator = new BelgiumIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsBE()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("BE");
    }

    [Theory]
    [InlineData("BE68539007547034")]
    [InlineData("BE71096123456769")]
    [InlineData("BE62510007547061")]
    public void IsValidIban_WithValidBelgianIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034")] // With spaces
    [InlineData("be68539007547034")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE00539007547034")] // Wrong check digits
    [InlineData("NL91ABNA0417164300")] // Dutch IBAN
    [InlineData("LU280019400644750000")] // Luxembourg IBAN
    [InlineData("BE685390075470")] // Too short (14 chars)
    [InlineData("BE6853900754703412")] // Too long (18 chars)
    [InlineData("BE6853900754703X")] // Invalid character
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

    #region Static Method Tests - ValidateBelgiumIban

    [Theory]
    [InlineData("BE68539007547034")]
    [InlineData("BE71096123456769")]
    [InlineData("BE62510007547061")]
    public void ValidateBelgianIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034")] // With spaces
    [InlineData("be68539007547034")] // Lowercase
    [InlineData("Be68 5390 0754 7034")] // Mixed case with spaces
    public void ValidateBelgianIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE00539007547034")] // Wrong check digits
    [InlineData("BE99999999999999")] // Invalid checksum
    [InlineData("NL91ABNA0417164300")] // Wrong country
    [InlineData("BE685390075470")] // Too short
    [InlineData("BE6853900754703412")] // Too long
    [InlineData("BE6X539007547034")] // Invalid character in check digits
    [InlineData("BE6853900754703X")] // Invalid character in account
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateBelgianIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ValidateBelgianIban_WithAllZeros_ReturnsFalse()
    {
        // Arrange
        var iban = "BE00000000000000";

        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateBelgianIban_WithOnlyLetters_ReturnsFalse()
    {
        // Arrange
        var iban = "BEABCDEFGHIJKLMN";

        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateBelgianIban_WithSpecialCharacters_ReturnsFalse()
    {
        // Arrange
        var iban = "BE68!539@0754#7034";

        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion
}

