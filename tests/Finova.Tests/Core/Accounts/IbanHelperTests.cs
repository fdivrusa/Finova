using Finova.Core.Iban;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Accounts;

public class IbanHelperTests
{
    #region IsValidIban Tests

    [Theory]
    [InlineData("BE68539007547034", true)]
    [InlineData("BE71096123456769", true)]
    [InlineData("NL91ABNA0417164300", true)]
    [InlineData("LU280019400644750000", true)]
    [InlineData("LU120010001234567891", true)]
    [InlineData("GB82WEST12345698765432", true)]
    [InlineData("FR1420041010050500013M02606", true)]
    public void IsValidIban_WithValidIbans_ReturnsTrue(string iban, bool expected)
    {
        // Act
        var result = IbanHelper.IsValidIban(iban);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("BE00539007547034")] // Wrong check digits
    [InlineData("NL00ABNA0417164300")] // Wrong check digits
    [InlineData("LU000019400644750000")] // Wrong check digits
    [InlineData("XX68539007547034")] // Invalid country code
    [InlineData("BE685390075470")] // Too short
    [InlineData("BE6853900754703412345")] // Too long
    [InlineData("")] // Empty
    [InlineData("   ")] // Whitespace only
    [InlineData(null)] // Null
    [InlineData("1234567890")] // No country code
    [InlineData("BE6X539007547034")] // Invalid character in check digits
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = IbanHelper.IsValidIban(iban);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034")] // With spaces
    [InlineData("be68539007547034")] // Lowercase
    public void IsValidIban_WithFormattedIbans_HandlesCorrectly(string iban)
    {
        // Act
        var result = IbanHelper.IsValidIban(iban);

        // Assert - Both spaces and lowercase should be accepted
        result.Should().BeTrue();
    }

    #endregion

    #region NormalizeIban Tests

    [Theory]
    [InlineData("BE68 5390 0754 7034", "BE68539007547034")]
    [InlineData("be68539007547034", "BE68539007547034")]
    [InlineData("Be68 5390 0754 7034", "BE68539007547034")]
    [InlineData("  BE68539007547034  ", "BE68539007547034")]
    [InlineData("NL91 ABNA 0417 1643 00", "NL91ABNA0417164300")]
    [InlineData("nl91abna0417164300", "NL91ABNA0417164300")]
    public void NormalizeIban_RemovesSpacesAndUppercases(string input, string expected)
    {
        // Act
        var result = IbanHelper.NormalizeIban(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NormalizeIban_WithEmptyInput_ReturnsEmpty(string? input)
    {
        // Act
        var result = IbanHelper.NormalizeIban(input);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region FormatIban Tests

    [Theory]
    [InlineData("BE68539007547034", "BE68 5390 0754 7034")]
    [InlineData("NL91ABNA0417164300", "NL91 ABNA 0417 1643 00")]
    [InlineData("LU280019400644750000", "LU28 0019 4006 4475 0000")]
    [InlineData("GB82WEST12345698765432", "GB82 WEST 1234 5698 7654 32")]
    public void FormatIban_AddsSpacesEveryFourCharacters(string input, string expected)
    {
        // Act
        var result = IbanHelper.FormatIban(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("be68539007547034", "BE68 5390 0754 7034")] // Lowercase
    [InlineData("BE68 5390 0754 7034", "BE68 5390 0754 7034")] // Already formatted
    public void FormatIban_NormalizesBeforeFormatting(string input, string expected)
    {
        // Act
        var result = IbanHelper.FormatIban(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void FormatIban_WithEmptyInput_ReturnsEmpty(string? input)
    {
        // Act
        var result = IbanHelper.FormatIban(input);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetCountryCode Tests

    [Theory]
    [InlineData("BE68539007547034", "BE")]
    [InlineData("NL91ABNA0417164300", "NL")]
    [InlineData("LU280019400644750000", "LU")]
    [InlineData("GB82WEST12345698765432", "GB")]
    [InlineData("FR1420041010050500013M02606", "FR")]
    public void GetCountryCode_ReturnsCorrectCode(string iban, string expected)
    {
        // Act
        var result = IbanHelper.GetCountryCode(iban);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034", "BE")] // With spaces
    [InlineData("be68539007547034", "BE")] // Lowercase
    public void GetCountryCode_NormalizesInput(string iban, string expected)
    {
        // Act
        var result = IbanHelper.GetCountryCode(iban);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("B")]
    [InlineData("1")]
    [InlineData(null)]
    public void GetCountryCode_WithInvalidInput_ReturnsEmpty(string? iban)
    {
        // Act
        var result = IbanHelper.GetCountryCode(iban);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetCheckDigits Tests

    [Theory]
    [InlineData("BE68539007547034", 68)]
    [InlineData("NL91ABNA0417164300", 91)]
    [InlineData("LU280019400644750000", 28)]
    [InlineData("BE71096123456769", 71)]
    public void GetCheckDigits_ReturnsCorrectDigits(string iban, int expected)
    {
        // Act
        var result = IbanHelper.GetCheckDigits(iban);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034", 68)] // With spaces
    [InlineData("be68539007547034", 68)] // Lowercase
    public void GetCheckDigits_NormalizesInput(string iban, int expected)
    {
        // Act
        var result = IbanHelper.GetCheckDigits(iban);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("BE")]
    [InlineData("BEX8539007547034")] // Non-digit check digits
    [InlineData(null)]
    public void GetCheckDigits_WithInvalidInput_ReturnsZero(string? iban)
    {
        // Act
        var result = IbanHelper.GetCheckDigits(iban);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region ValidateChecksum Tests

    [Theory]
    [InlineData("BE68539007547034", true)]
    [InlineData("BE71096123456769", true)]
    [InlineData("NL91ABNA0417164300", true)]
    [InlineData("LU280019400644750000", true)]
    public void ValidateChecksum_WithValidIbans_ReturnsTrue(string iban, bool expected)
    {
        // Act
        var result = IbanHelper.ValidateChecksum(iban);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("BE00539007547034")] // Wrong check digits
    [InlineData("NL00ABNA0417164300")] // Wrong check digits
    [InlineData("BE99999999999999")] // Invalid checksum
    public void ValidateChecksum_WithInvalidChecksums_ReturnsFalse(string iban)
    {
        // Act
        var result = IbanHelper.ValidateChecksum(iban);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("BE")]
    [InlineData("BE68")]
    public void ValidateChecksum_WithInvalidInput_ReturnsFalse(string? iban)
    {
        // Act
        var result = IbanHelper.ValidateChecksum(iban);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ValidateChecksum_WithInvalidCharacters_ReturnsFalse()
    {
        // Arrange
        // Contains '-' which is not alphanumeric, so ConvertLettersToDigits throws ArgumentException
        // ValidateChecksum catches it and returns false
        var invalidIban = "BE68-5390-0754-7034";

        // Act
        var result = IbanHelper.ValidateChecksum(invalidIban);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void IsValidIban_WithVeryLongIban_HandlesCorrectly()
    {
        // Arrange - Maximum IBAN length is 34 characters
        var validLongIban = "MT84MALT011000012345MTLCAST001S"; // Malta - 31 chars

        // Act
        var result = IbanHelper.IsValidIban(validLongIban);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithMinimumLengthIban_HandlesCorrectly()
    {
        // Arrange - Minimum IBAN length is 15 characters (Norway)
        var validShortIban = "NO9386011117947"; // Norway - 15 chars

        // Act
        var result = IbanHelper.IsValidIban(validShortIban);

        // Assert
        result.Should().BeTrue();
    }

    #endregion
}
