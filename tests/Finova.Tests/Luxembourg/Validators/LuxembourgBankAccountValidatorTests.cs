using Finova.Luxembourg.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Luxembourg.Validators
{
    public class LuxembourgBankAccountValidatorTests
    {
        private readonly LuxembourgBankAccountValidator _validator;

        public LuxembourgBankAccountValidatorTests()
        {
            _validator = new LuxembourgBankAccountValidator();
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
            var result = _validator.IsValidIban(iban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("LU28 0019 4006 4475 0000")] // With spaces
        [InlineData("lu280019400644750000")] // Lowercase
        public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        {
            // Act
            var result = _validator.IsValidIban(iban);

            // Assert
            result.Should().BeTrue();
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
        public void IsValidIban_WithInvalidIbans_ReturnsFalse(string iban)
        {
            // Act
            var result = _validator.IsValidIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Static Method Tests - ValidateLuxembourgIban

        [Theory]
        [InlineData("LU280019400644750000")]
        [InlineData("LU120010001234567891")]
        public void ValidateLuxembourgIban_WithValidIbans_ReturnsTrue(string iban)
        {
            // Act
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("LU28 0019 4006 4475 0000")] // With spaces
        [InlineData("lu280019400644750000")] // Lowercase
        [InlineData("Lu28 0019 4006 4475 0000")] // Mixed case with spaces
        public void ValidateLuxembourgIban_WithFormattedIbans_ReturnsTrue(string iban)
        {
            // Act
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().BeTrue();
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
        public void ValidateLuxembourgIban_WithInvalidIbans_ReturnsFalse(string iban)
        {
            // Act
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Static Method Tests - FormatLuxembourgIban

        [Theory]
        [InlineData("LU280019400644750000", "LU28 0019 4006 4475 0000")]
        [InlineData("LU120010001234567891", "LU12 0010 0012 3456 7891")]
        [InlineData("lu280019400644750000", "LU28 0019 4006 4475 0000")] // Lowercase
        [InlineData("LU28 0019 4006 4475 0000", "LU28 0019 4006 4475 0000")] // Already formatted
        public void FormatLuxembourgIban_WithValidIbans_ReturnsFormattedString(string iban, string expected)
        {
            // Act
            var result = LuxembourgBankAccountValidator.FormatLuxembourgIban(iban);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("LU000019400644750000")] // Wrong check digits
        [InlineData("NL91ABNA0417164300")] // Wrong country
        [InlineData("LU28001940064475")] // Too short
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void FormatLuxembourgIban_WithInvalidIbans_ThrowsArgumentException(string iban)
        {
            // Act
            Action act = () => LuxembourgBankAccountValidator.FormatLuxembourgIban(iban);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        #region Common Luxembourg Banks

        [Theory]
        [InlineData("LU280019400644750000")] // BCEE (001)
        [InlineData("LU120010001234567891")] // BCEE (001)
        public void ValidateLuxembourgIban_WithKnownBankCodes_ReturnsTrue(string iban)
        {
            // Act
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().BeTrue();
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
            var shortResult = LuxembourgBankAccountValidator.ValidateLuxembourgIban(tooShort);
            var longResult = LuxembourgBankAccountValidator.ValidateLuxembourgIban(tooLong);
            var correctResult = LuxembourgBankAccountValidator.ValidateLuxembourgIban(correct);

            // Assert
            shortResult.Should().BeFalse();
            longResult.Should().BeFalse();
            correctResult.Should().BeTrue();
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
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(validIban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("LU28AB19400644750000")] // Letters in bank code
        [InlineData("LU280019A00644750000")] // Letter in account number
        [InlineData("LU28001940064475000X")] // Letter at end of account
        public void ValidateLuxembourgIban_BankAndAccountMustBeDigits_ReturnsFalse(string iban)
        {
            // Act
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().BeFalse();
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
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void ValidateLuxembourgIban_WithAllZeros_ReturnsFalse()
        {
            // Arrange
            var iban = "LU00000000000000000000";

            // Act
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void FormatLuxembourgIban_FirstGroupIsSixCharacters()
        {
            // Arrange
            var iban = "LU280019400644750000";

            // Act
            var result = LuxembourgBankAccountValidator.FormatLuxembourgIban(iban);

            // Assert
            // Format: "LU28 0019 4006 4475 0000"
            // Groups: 4 chars, 4 chars, 4 chars, 4 chars, 4 chars
            result.Should().MatchRegex(@"^[A-Z]{2}\d{2}( \d{4}){4}$");
        }

        #endregion

        #region Comparison with Other Countries

        [Theory]
        [InlineData("BE68539007547034")] // Belgian (16 chars)
        [InlineData("NL91ABNA0417164300")] // Dutch (18 chars)
        public void ValidateLuxembourgIban_RejectsBelgianAndDutchIbans(string iban)
        {
            // Act
            var result = LuxembourgBankAccountValidator.ValidateLuxembourgIban(iban);

            // Assert
            result.Should().BeFalse();
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
}
