using Finova.Core.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Services
{
    public class IbanServiceTests
    {
        private readonly IbanService _service;

        public IbanServiceTests()
        {
            _service = new IbanService();
        }

        #region IsValidIban Tests

        [Theory]
        [InlineData("BE68539007547034")]
        [InlineData("NL91ABNA0417164300")]
        [InlineData("LU280019400644750000")]
        [InlineData("GB29NWBK60161331926819")]
        [InlineData("FR1420041010050500013M02606")]
        [InlineData("DE89370400440532013000")]
        public void IsValidIban_WithValidIbans_ReturnsTrue(string iban)
        {
            // Act
            var result = _service.IsValidIban(iban);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("BE00539007547034")] // Wrong check digits
        [InlineData("XX68539007547034")] // Invalid country
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        {
            // Act
            var result = _service.IsValidIban(iban);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region FormatIban Tests

        [Theory]
        [InlineData("BE68539007547034", "BE68 5390 0754 7034")]
        [InlineData("NL91ABNA0417164300", "NL91 ABNA 0417 1643 00")]
        [InlineData("DE89370400440532013000", "DE89 3704 0044 0532 0130 00")]
        public void FormatIban_FormatsCorrectly(string iban, string expected)
        {
            // Act
            var result = _service.FormatIban(iban);

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region NormalizeIban Tests

        [Theory]
        [InlineData("BE68 5390 0754 7034", "BE68539007547034")]
        [InlineData("be68539007547034", "BE68539007547034")]
        public void NormalizeIban_NormalizesCorrectly(string iban, string expected)
        {
            // Act
            var result = _service.NormalizeIban(iban);

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region GetCountryCode Tests

        [Theory]
        [InlineData("BE68539007547034", "BE")]
        [InlineData("NL91ABNA0417164300", "NL")]
        [InlineData("GB29NWBK60161331926819", "GB")]
        public void GetCountryCode_ReturnsCorrectCode(string iban, string expected)
        {
            // Act
            var result = _service.GetCountryCode(iban);

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region GetCheckDigits Tests

        [Theory]
        [InlineData("BE68539007547034", 68)]
        [InlineData("NL91ABNA0417164300", 91)]
        [InlineData("GB29NWBK60161331926819", 29)]
        public void GetCheckDigits_ReturnsCorrectDigits(string iban, int expected)
        {
            // Act
            var result = _service.GetCheckDigits(iban);

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region ValidateChecksum Tests

        [Theory]
        [InlineData("BE68539007547034", true)]
        [InlineData("BE00539007547034", false)]
        public void ValidateChecksum_ValidatesCorrectly(string iban, bool expected)
        {
            // Act
            var result = _service.ValidateChecksum(iban);

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region CountryCode Tests

        [Fact]
        public void CountryCode_ReturnsNull()
        {
            // Act
            var result = _service.CountryCode;

            // Assert
            result.Should().BeNull("Generic service doesn't have a specific country");
        }

        #endregion
    }
}
