using Finova.Netherlands.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Netherlands.Validators
{
    public class DutchBankAccountValidatorTests
    {
        private readonly DutchBankAccountValidator _validator;

        public DutchBankAccountValidatorTests()
        {
            _validator = new DutchBankAccountValidator();
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
            var result = _validator.IsValidIban(iban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("NL91 ABNA 0417 1643 00")] // With spaces
        [InlineData("nl91abna0417164300")] // Lowercase
        public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        {
            // Act
            var result = _validator.IsValidIban(iban);

            // Assert
            result.Should().BeTrue();
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
        public void IsValidIban_WithInvalidIbans_ReturnsFalse(string iban)
        {
            // Act
            var result = _validator.IsValidIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Static Method Tests - ValidateDutchIban

        [Theory]
        [InlineData("NL91ABNA0417164300")]
        public void ValidateDutchIban_WithValidIbans_ReturnsTrue(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("NL91 ABNA 0417 1643 00")] // With spaces
        [InlineData("nl91abna0417164300")] // Lowercase
        [InlineData("Nl91 Abna 0417 1643 00")] // Mixed case with spaces
        public void ValidateDutchIban_WithFormattedIbans_ReturnsTrue(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeTrue();
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
        public void ValidateDutchIban_WithInvalidIbans_ReturnsFalse(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Static Method Tests - GetBankCode

        [Theory]
        [InlineData("NL91ABNA0417164300", "ABNA")]
        public void GetBankCode_WithValidIbans_ReturnsCorrectBankCode(string iban, string expectedBankCode)
        {
            // Act
            var result = DutchBankAccountValidator.GetBankCode(iban);

            // Assert
            result.Should().Be(expectedBankCode);
        }

        [Theory]
        [InlineData("NL91 ABNA 0417 1643 00", "ABNA")] // With spaces
        [InlineData("nl91abna0417164300", "ABNA")] // Lowercase - returns uppercase
        public void GetBankCode_WithFormattedIbans_ReturnsNormalizedBankCode(string iban, string expectedBankCode)
        {
            // Act
            var result = DutchBankAccountValidator.GetBankCode(iban);

            // Assert
            result.Should().Be(expectedBankCode);
        }

        [Theory]
        [InlineData("NL00ABNA0417164300")] // Invalid IBAN
        [InlineData("BE68539007547034")] // Wrong country
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void GetBankCode_WithInvalidIbans_ReturnsNull(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.GetBankCode(iban);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region Static Method Tests - FormatDutchIban

        [Theory]
        [InlineData("NL91ABNA0417164300", "NL91 ABNA 0417 1643 00")]
        [InlineData("nl91abna0417164300", "NL91 ABNA 0417 1643 00")] // Lowercase
        [InlineData("NL91 ABNA 0417 1643 00", "NL91 ABNA 0417 1643 00")] // Already formatted
        public void FormatDutchIban_WithValidIbans_ReturnsFormattedString(string iban, string expected)
        {
            // Act
            var result = DutchBankAccountValidator.FormatDutchIban(iban);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("NL00ABNA0417164300")] // Wrong check digits
        [InlineData("BE68539007547034")] // Wrong country
        [InlineData("NL91ABNA04171643")] // Too short
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void FormatDutchIban_WithInvalidIbans_ThrowsArgumentException(string iban)
        {
            // Act
            Action act = () => DutchBankAccountValidator.FormatDutchIban(iban);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        #region Bank Code Validation Tests

        [Theory]
        [InlineData("NL91ABNA0417164300")] // All letters
        public void ValidateDutchIban_BankCodeMustBeLetters_ReturnsTrue(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("NL91AB1A0417164300")] // Bank code with digit
        [InlineData("NL911BNA0417164300")] // Bank code with digit
        [InlineData("NL91A2NA0417164300")] // Bank code with digit
        [InlineData("NL91ABN30417164300")] // Bank code with digit
        public void ValidateDutchIban_BankCodeWithDigits_ReturnsFalse(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Account Number Validation Tests

        [Theory]
        [InlineData("NL91ABNA0417164300")] // All digits
        public void ValidateDutchIban_AccountNumberMustBeDigits_ReturnsTrue(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("NL91ABNA041716430X")] // Account with letter at end
        [InlineData("NL91ABNA0417A64300")] // Account with letter in middle
        [InlineData("NL91ABNA041716X300")] // Account with letter
        public void ValidateDutchIban_AccountNumberWithLetters_ReturnsFalse(string iban)
        {
            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Common Dutch Banks

        [Theory]
        [InlineData("NL91ABNA0417164300", "ABNA")] // ABN AMRO
        public void GetBankCode_IdentifiesCommonDutchBanks(string iban, string expectedBank)
        {
            // Act
            var bankCode = DutchBankAccountValidator.GetBankCode(iban);

            // Assert
            bankCode.Should().Be(expectedBank);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void ValidateDutchIban_WithAllZerosInAccountNumber_ReturnsFalse()
        {
            // Arrange
            var iban = "NL00ABNA0000000000";

            // Act
            var result = DutchBankAccountValidator.ValidateDutchIban(iban);

            // Assert
            result.Should().BeFalse(); // Invalid checksum
        }

        [Fact]
        public void GetBankCode_ReturnsExactlyFourCharacters()
        {
            // Arrange
            var iban = "NL91ABNA0417164300";

            // Act
            var bankCode = DutchBankAccountValidator.GetBankCode(iban);

            // Assert
            bankCode.Should().HaveLength(4);
        }

        #endregion
    }
}
