using Finova.Countries.Europe.Netherlands.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Netherlands.Validators;

public class DutchBankAccountValidatorTests
{
    private readonly NetherlandsIbanValidator _validator;

    public DutchBankAccountValidatorTests()
    {
        _validator = new NetherlandsIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsNL()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("NL");
    }

    [Theory]
    [InlineData("NL91ABNA0417164300")]
    public void IsValidIban_WithValidDutchIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NL91 ABNA 0417 1643 00")] // With spaces
    [InlineData("nl91abna0417164300")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NL00ABNA0417164300")] // Wrong check digits
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("LU280019400644750000")] // Luxembourg IBAN
    [InlineData("NL91ABNA04171643")] // Too short (16 chars)
    [InlineData("NL91ABNA041716430012")] // Too long (20 chars)
    [InlineData("NL91123A0417164300")] // Invalid bank code (not all letters)
    [InlineData("NL91ABNA041716430X")] // Invalid account number (not all digits)
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

    #region Static Method Tests - ValidateNetherlandsIban

    [Theory]
    [InlineData("NL91ABNA0417164300")]
    public void ValidateDutchIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NL91 ABNA 0417 1643 00")] // With spaces
    [InlineData("nl91abna0417164300")] // Lowercase
    [InlineData("Nl91 Abna 0417 1643 00")] // Mixed case with spaces
    public void ValidateDutchIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NL00ABNA0417164300")] // Wrong check digits
    [InlineData("NL00ZZZZ9999999999")] // Invalid bank code (non-existent)
    [InlineData("BE68539007547034")] // Wrong country
    [InlineData("NL91ABNA04171643")] // Too short
    [InlineData("NL91ABNA041716430012")] // Too long
    [InlineData("NL9XABNA0417164300")] // Invalid character in check digits
    [InlineData("NL91AB1A0417164300")] // Bank code contains digit
    [InlineData("NL91ABNA041716430X")] // Account number contains letter
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateDutchIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Bank Code Validation Tests

    [Theory]
    [InlineData("NL91ABNA0417164300")] // All letters
    public void ValidateDutchIban_BankCodeMustBeLetters_ReturnsTrue(string iban)
    {
        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NL91AB1A0417164300")] // Bank code with digit
    [InlineData("NL911BNA0417164300")] // Bank code with digit
    [InlineData("NL91A2NA0417164300")] // Bank code with digit
    [InlineData("NL91ABN30417164300")] // Bank code with digit
    public void ValidateDutchIban_BankCodeWithDigits_ReturnsFalse(string iban)
    {
        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Account Number Validation Tests

    [Theory]
    [InlineData("NL91ABNA0417164300")] // All digits
    public void ValidateDutchIban_AccountNumberMustBeDigits_ReturnsTrue(string iban)
    {
        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NL91ABNA041716430X")] // Account with letter at end
    [InlineData("NL91ABNA0417A64300")] // Account with letter in middle
    [InlineData("NL91ABNA041716X300")] // Account with letter
    public void ValidateDutchIban_AccountNumberWithLetters_ReturnsFalse(string iban)
    {
        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ValidateDutchIban_WithAllZerosInAccountNumber_ReturnsFalse()
    {
        // Arrange
        var iban = "NL00ABNA0000000000";

        // Act
        var result = NetherlandsIbanValidator.ValidateNetherlandsIban(iban);

        // Assert
        result.IsValid.Should().BeFalse(); // Invalid checksum
    }
    #endregion
}


