using Finova.UnitedKingdom.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.UnitedKingdom.Validators
{
    public class UnitedKingdomIbanValidatorTests
    {
        private readonly UnitedKingdomIbanValidator _validator = new();

        [Fact]
        public void CountryCode_ReturnsGB() => _validator.CountryCode.Should().Be("GB");

        [Theory]
        [InlineData("GB29NWBK60161331926819")]
        [InlineData("GB82WEST12345698765432")]
        public void IsValidIban_WithValidUKIbans_ReturnsTrue(string iban)
            => _validator.IsValidIban(iban).Should().BeTrue();

        [Theory]
        [InlineData("GB29 NWBK 6016 1331 9268 19")] // With spaces
        [InlineData("gb29nwbk60161331926819")] // Lowercase
        public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
            => _validator.IsValidIban(iban).Should().BeTrue();

        [Theory]
        [InlineData("GB00NWBK60161331926819")] // Wrong check digits
        [InlineData("FR1420041010050500013M02606")] // Wrong country
        [InlineData("GB29NWBK601613319268")] // Too short
        [InlineData("GB29NWBK6016133192681999")] // Too long
        [InlineData("GB29NW1K60161331926819")] // Digit in bank code
        [InlineData("GB29NWBK6016133192681X")] // Letter in account
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
            => _validator.IsValidIban(iban).Should().BeFalse();

        [Theory]
        [InlineData("GB29NWBK60161331926819")]
        public void ValidateUnitedKingdomIban_WithValidIbans_ReturnsTrue(string iban)
            => UnitedKingdomIbanValidator.ValidateUnitedKingdomIban(iban).Should().BeTrue();

        [Fact]
        public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
        {
            var iban = "GB29NWBK60161331926819";
            var result1 = _validator.IsValidIban(iban);
            var result2 = _validator.IsValidIban(iban);
            result1.Should().Be(result2);
        }
    }
}
