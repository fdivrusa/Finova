using Finova.Countries.Europe.Ireland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ireland.Validators;

public class IrelandIbanValidatorTests
{
    private readonly IrelandIbanValidator _validator;

    public IrelandIbanValidatorTests()
    {
        _validator = new IrelandIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsIE()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("IE");
    }

    [Theory]
    [InlineData("IE29AIBK93115212345678")] // AIB Bank example
    [InlineData("IE64IRCE92050112345678")] // Irish Permanent example
    public void IsValidIban_WithValidIrishIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IE29 AIBK 9311 5212 3456 78")] // With spaces
    [InlineData("ie29aibk93115212345678")] // Lowercase
    [InlineData("Ie29 AIBK 9311 5212 3456 78")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IE00AIBK93115212345678")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("IE29AIBK931152123456")] // Too short (20 chars)
    [InlineData("IE29AIBK9311521234567890")] // Too long (24 chars)
    [InlineData("IE29A1BK93115212345678")] // Digit in bank code (position 5)
    [InlineData("IE29AIBK9311521234567X")] // Letter in account number
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
    [InlineData("IE29AIBK93115212345678")]
    [InlineData("IE64IRCE92050112345678")]
    public void ValidateIrelandIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = IrelandIbanValidator.ValidateIrelandIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IE00AIBK93115212345678")] // Wrong check digits
    [InlineData("GB29NWBK60161331926819")] // Wrong country (UK)
    [InlineData("IE29AIBK931152123456")] // Too short
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateIrelandIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = IrelandIbanValidator.ValidateIrelandIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_ValidatesCountryPrefix()
    {
        // Wrong country prefix but valid structure otherwise
        var result = _validator.Validate("GB29NWBK60161331926819");
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_ValidatesLength()
    {
        // Irish IBAN must be exactly 22 characters
        var shortIban = "IE29AIBK9311521234";
        var longIban = "IE29AIBK931152123456789";

        _validator.Validate(shortIban).IsValid.Should().BeFalse();
        _validator.Validate(longIban).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("IE29A1BK93115212345678")] // Digit at position 5 (should be letter)
    [InlineData("IE291IBK93115212345678")] // Digit at position 4 (should be letter)
    [InlineData("IE29AIBK9311521234567A")] // Letter at position 21 (should be digit)
    public void IsValidIban_ValidatesCharacterPositions(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_BankCodeMustBeLetters()
    {
        // Bank code (positions 4-7) must be all letters
        var invalidBankCode = "IE291234931152123456789";

        var result = _validator.Validate(invalidBankCode);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_SortCodeAndAccountMustBeDigits()
    {
        // Sort code (8-13) and account (14-21) must be digits
        var invalidSortCode = "IE29AIBK9A115212345678"; // Letter 'A' in sort code

        var result = _validator.Validate(invalidSortCode);

        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Edge Cases

    [Theory]
    [InlineData("IE29AIBK93115212345678")] // Standard valid
    [InlineData("IE64IRCE92050112345678")] // Different bank
    public void IsValidIban_WithDifferentValidBanks_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithAllZerosSortCodeAndAccount_ChecksModulo97()
    {
        // Even with zeros, the modulo-97 check must pass
        var ibanWithZeros = "IE00AIBK00000000000000";

        var result = _validator.Validate(ibanWithZeros);

        // This should fail because check digits "00" are invalid
        result.IsValid.Should().BeFalse();
    }

    #endregion
}



