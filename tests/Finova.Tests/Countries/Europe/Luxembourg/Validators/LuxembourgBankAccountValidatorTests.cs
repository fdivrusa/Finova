using Finova.Countries.Europe.Luxembourg.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Luxembourg.Validators;

public class LuxembourgBankAccountValidatorTests
{
    private readonly LuxembourgIbanValidator _validator;

    public LuxembourgBankAccountValidatorTests()
    {
        _validator = new LuxembourgIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsLU()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("LU");
    }

    [Theory]
    [InlineData("LU280019400644750000")]
    [InlineData("LU120010001234567891")]
    public void IsValidIban_WithValidLuxembourgIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LU28 0019 4006 4475 0000")] // With spaces
    [InlineData("lu280019400644750000")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LU000019400644750000")] // Wrong check digits
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("NL91ABNA0417164300")] // Dutch IBAN
    [InlineData("LU28001940064475")] // Too short (16 chars)
    [InlineData("LU280019400644750000123")] // Too long (23 chars)
    [InlineData("LU2800194006447500X0")] // Invalid character
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

    #region Static Method Tests - ValidateLuxembourgIban

    [Theory]
    [InlineData("LU280019400644750000")]
    [InlineData("LU120010001234567891")]
    public void ValidateLuxembourgIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LU28 0019 4006 4475 0000")] // With spaces
    [InlineData("lu280019400644750000")] // Lowercase
    [InlineData("Lu28 0019 4006 4475 0000")] // Mixed case with spaces
    public void ValidateLuxembourgIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LU000019400644750000")] // Wrong check digits
    [InlineData("LU999999999999999999")] // Invalid checksum
    [InlineData("NL91ABNA0417164300")] // Wrong country
    [InlineData("LU28001940064475")] // Too short
    [InlineData("LU280019400644750000123")] // Too long
    [InlineData("LU2X0019400644750000")] // Invalid character in check digits
    [InlineData("LU280019400644750X00")] // Invalid character in account
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateLuxembourgIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateLuxembourgIban_WithCorrectLengthButWrongCountryCode_ReturnsFalse()
    {
        // Arrange
        var invalidIban = "BE280019400644750000"; // 20 chars, starts with BE

        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(invalidIban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Common Luxembourg Banks

    [Theory]
    [InlineData("LU280019400644750000")] // BCEE (001)
    [InlineData("LU120010001234567891")] // BCEE (001)
    public void ValidateLuxembourgIban_WithKnownBankCodes_ReturnsTrue(string iban)
    {
        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Length Validation Tests

    [Fact]
    public void ValidateLuxembourgIban_MustBeExactly20Characters()
    {
        // Arrange
        var tooShort = "LU28001940064475"; // 16 chars
        var tooLong = "LU280019400644750000123"; // 23 chars
        var correct = "LU280019400644750000"; // 20 chars

        // Act
        var shortResult = LuxembourgIbanValidator.ValidateLuxembourgIban(tooShort);
        var longResult = LuxembourgIbanValidator.ValidateLuxembourgIban(tooLong);
        var correctResult = LuxembourgIbanValidator.ValidateLuxembourgIban(correct);

        // Assert
        shortResult.IsValid.Should().BeFalse();
        longResult.IsValid.Should().BeFalse();
        correctResult.IsValid.Should().BeTrue();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void ValidateLuxembourgIban_ValidatesStructure()
    {
        // Arrange
        var validIban = "LU280019400644750000";
        // Structure: LU (country) + 28 (check) + 001 (bank) + 9400644750000 (account)

        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(validIban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LU28AB19400644750000")] // Letters in bank code
    [InlineData("LU280019A00644750000")] // Letter in account number
    [InlineData("LU28001940064475000X")] // Letter at end of account
    public void ValidateLuxembourgIban_BankAndAccountMustBeDigits_ReturnsFalse(string iban)
    {
        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Checksum Validation Tests

    [Theory]
    [InlineData("LU280019400644750000", true)]
    [InlineData("LU000019400644750000", false)] // Wrong check digits
    [InlineData("LU990019400644750000", false)] // Invalid checksum
    public void ValidateLuxembourgIban_ValidatesChecksum(string iban, bool expected)
    {
        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().Be(expected);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ValidateLuxembourgIban_WithAllZeros_ReturnsFalse()
    {
        // Arrange
        var iban = "LU00000000000000000000";

        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Comparison with Other Countries

    [Theory]
    [InlineData("BE68539007547034")] // Belgian (16 chars)
    [InlineData("NL91ABNA0417164300")] // Dutch (18 chars)
    public void ValidateLuxembourgIban_RejectsBelgianAndDutchIbans(string iban)
    {
        // Act
        var result = LuxembourgIbanValidator.ValidateLuxembourgIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void LuxembourgIban_IsLongerThanBelgianAndDutch()
    {
        // Arrange
        var belgianLength = 16;
        var dutchLength = 18;
        var luxembourgLength = 20;

        // Assert
        luxembourgLength.Should().BeGreaterThan(belgianLength);
        luxembourgLength.Should().BeGreaterThan(dutchLength);
    }

    #endregion
}

