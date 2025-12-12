using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Validators;

public class BelgianEnterpriseValidatorTests
{
    #region IsValid Tests

    [Theory]
    [InlineData("0123456749")] // Valid: 97 - (12345679 % 97) = 49 (but calculation: 12345679 % 97 = 48, so 97-48=49)
    [InlineData("0403170701")] // Valid KBO
    public void IsValid_WithValidKboNumbers_ReturnsTrue(string kbo)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Validate(kbo);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("0123.456.749")] // With dots
    [InlineData("BE0123456749")] // With BE prefix
    [InlineData("BE 0123.456.749")] // With BE prefix and dots
    [InlineData(" 0123456749 ")] // With whitespace
    public void IsValid_WithFormattedKboNumbers_ReturnsTrue(string kbo)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Validate(kbo);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("0123456700")] // Wrong check digits
    [InlineData("0000000000")] // All zeros
    [InlineData("2123456749")] // Doesn't start with 0 or 1
    [InlineData("9123456749")] // Doesn't start with 0 or 1
    [InlineData("012345674")] // Too short (9 digits)
    [InlineData("01234567490")] // Too long (11 digits)
    [InlineData("012345674X")] // Contains letter
    [InlineData("")] // Empty
    [InlineData("   ")] // Whitespace
    [InlineData(null)] // Null
    public void IsValid_WithInvalidKboNumbers_ReturnsFalse(string? kbo)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Validate(kbo);
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Format Tests

    [Theory]
    [InlineData("0123456749", "0123.456.749")]
    [InlineData("0403170701", "0403.170.701")]
    public void Format_WithValidKboNumbers_ReturnsFormattedString(string kbo, string expected)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Format(kbo);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("0123.456.749", "0123.456.749")] // Already formatted
    [InlineData("BE0123456749", "0123.456.749")] // With BE prefix
    [InlineData(" 0123456749 ", "0123.456.749")] // With whitespace
    public void Format_WithFormattedInput_ReturnsConsistentFormat(string kbo, string expected)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Format(kbo);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("0123456700")] // Invalid check digits
    [InlineData("012345674")] // Too short
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void Format_WithInvalidKboNumbers_ThrowsArgumentException(string? kbo)
    {
        // Act
        Action act = () => BelgiumEnterpriseValidator.Format(kbo);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Normalize Tests

    [Theory]
    [InlineData("0123.456.749", "0123456749")]
    [InlineData("BE0123456749", "0123456749")]
    [InlineData("BE 0123.456.749", "0123456749")]
    [InlineData(" 0123456749 ", "0123456749")]
    [InlineData("0123456749", "0123456749")] // Already normalized
    public void Normalize_RemovesBEPrefixDotsAndSpaces(string input, string expected)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Normalize_WithEmptyInput_ReturnsEmpty(string? input)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Normalize(input);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region Modulo 97 Validation Tests

    [Fact]
    public void IsValid_ValidatesModulo97CheckDigits()
    {
        // Arrange - Calculate valid check digits
        // Example: 12345678 -> 12345678 % 97 = 61 -> 97 - 61 = 36
        var baseNumber = "01234567";
        var remainder = int.Parse(baseNumber) % 97;
        var checkDigits = (97 - remainder).ToString("D2");
        var validKbo = baseNumber + checkDigits;

        // Act
        var result = BelgiumEnterpriseValidator.Validate(validKbo);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithIncorrectCheckDigits_ReturnsFalse()
    {
        // Arrange - Use incorrect check digits
        var baseNumber = "01234567";
        var remainder = int.Parse(baseNumber) % 97;
        var correctCheckDigits = 97 - remainder;
        var incorrectCheckDigits = (correctCheckDigits + 1) % 100; // Wrong digits
        var invalidKbo = baseNumber + incorrectCheckDigits.ToString("D2");

        // Act
        var result = BelgiumEnterpriseValidator.Validate(invalidKbo);
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Real-World KBO Numbers

    [Theory]
    [InlineData("0403170701")] // Microsoft Belgium
    public void IsValid_WithRealKboNumbers_ReturnsTrue(string kbo)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Validate(kbo);
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void IsValid_WithStartingOne_ReturnsTrue()
    {
        // Arrange - Valid KBO starting with 1 (calculate properly)
        // 12345678 % 97 = 0, so check digits = 97
        var kbo = "1019755159";

        // Act
        var result = BelgiumEnterpriseValidator.Validate(kbo);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithCheckDigits97_HandlesCorrectly()
    {
        // Arrange - When modulo result is 0, check digits should be 97
        var baseNumber = "00000097"; // 97 % 97 = 0, so check digits = 97
        var kbo = baseNumber + "97";

        // Act
        var result = BelgiumEnterpriseValidator.Validate(kbo);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Format_PreservesLeadingZeros()
    {
        // Arrange - Use valid KBO
        var kbo = "0123456749"; // Valid one from our test data

        // Act
        var result = BelgiumEnterpriseValidator.Format(kbo);

        // Assert
        result.Should().Be("0123.456.749");
        result.Should().StartWith("0123");
    }

    #endregion
}

