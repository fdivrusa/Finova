using Finova.Countries.Europe.Norway.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Norway.Validators;

public class NorwayIbanValidatorTests
{
    private readonly NorwayIbanValidator _validator;

    public NorwayIbanValidatorTests()
    {
        _validator = new NorwayIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsNO()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("NO");
    }

    [Theory]
    [InlineData("NO9386011117947")] // DNB Bank example
    public void IsValidIban_WithValidNorwegianIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("NO93 8601 1117 947")] // With spaces
    [InlineData("no9386011117947")] // Lowercase
    [InlineData("No93 8601 1117 947")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("NO0086011117947")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("NO938601111794")] // Too short (14 chars)
    [InlineData("NO93860111179471")] // Too long (16 chars)
    [InlineData("NO9386011117XXX")] // Contains letters in account
    [InlineData("")] // Empty
    [InlineData("   ")] // Whitespace
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Static Method Tests

    [Theory]
    [InlineData("NO9386011117947")]
    public void ValidateNorwayIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = NorwayIbanValidator.ValidateNorwayIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("NO93 8601 1117 947")] // With spaces
    [InlineData("no9386011117947")] // Lowercase
    public void ValidateNorwayIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = NorwayIbanValidator.ValidateNorwayIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("NO0086011117947")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("NO938601111794")] // Too short
    [InlineData("NO93860111179471")] // Too long
    [InlineData("NO9386011117XXX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateNorwayIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = NorwayIbanValidator.ValidateNorwayIban(iban);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly15Characters_ReturnsTrue()
    {
        // Norwegian IBANs must be exactly 15 characters
        var iban = "NO9386011117947"; // 15 chars

        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(15);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("DE89370400440532013000")] // German IBAN
    public void IsValidIban_WithOtherCountryIbans_ReturnsFalse(string iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_VerifiesAllDigitsAfterCountryCode()
    {
        // Norwegian IBANs must be all digits after NO
        var validIban = "NO9386011117947";

        // Act
        var result = _validator.IsValidIban(validIban);

        // Assert
        result.Should().BeTrue();

        // Verify structure
        var normalized = validIban.ToUpper();
        normalized[2..15].All(char.IsDigit).Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "NO9386011117947";

        // Act
        var result1 = _validator.IsValidIban(iban);
        var result2 = _validator.IsValidIban(iban);

        // Assert
        result1.Should().Be(result2);
    }

    #endregion

    #region Norwegian-Specific Modulo 11 Validation Tests

    [Fact]
    public void IsValidIban_WithInvalidMod11Checksum_ReturnsFalse()
    {
        // Norwegian IBAN with invalid Modulo 11 check digit should fail
        // Changing the last digit invalidates the Mod11 checksum
        var invalidIban = "NO9386011117948"; // Changed last digit

        // Act
        var result = _validator.IsValidIban(invalidIban);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("NO9386011117947")] // Valid with correct Mod11 checksum
    [InlineData("NO8330001234567")] // Valid Nordea with correct Mod11
    public void IsValidIban_WithValidMod11Checksum_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.IsValidIban(iban);

        // Assert
        result.Should().BeTrue();
    }

    #endregion
}
