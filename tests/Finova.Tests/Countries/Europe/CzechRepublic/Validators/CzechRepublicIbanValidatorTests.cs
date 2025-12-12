using Finova.Countries.Europe.CzechRepublic.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.CzechRepublic.Validators;

public class CzechRepublicIbanValidatorTests
{
    private readonly CzechRepublicIbanValidator _validator;

    public CzechRepublicIbanValidatorTests()
    {
        _validator = new CzechRepublicIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsCZ()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("CZ");
    }

    [Theory]
    [InlineData("CZ6508000000192000145399")] // Česká spořitelna example
    [InlineData("CZ9455000000001011038930")] // Raiffeisen example
    public void IsValidIban_WithValidCzechIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CZ65 0800 0000 1920 0014 5399")] // With spaces
    [InlineData("cz6508000000192000145399")] // Lowercase
    [InlineData("Cz65 0800 0000 1920 0014 5399")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CZ0008000000192000145399")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("CZ65080000001920001453")] // Too short (22 chars)
    [InlineData("CZ650800000019200014539912")] // Too long (26 chars)
    [InlineData("CZ6508000000192000145XXX")] // Contains letters in account
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
    [InlineData("CZ6508000000192000145399")]
    [InlineData("CZ9455000000001011038930")]
    public void ValidateCzechIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = CzechRepublicIbanValidator.ValidateCzechIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CZ65 0800 0000 1920 0014 5399")] // With spaces
    [InlineData("cz6508000000192000145399")] // Lowercase
    public void ValidateCzechIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = CzechRepublicIbanValidator.ValidateCzechIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CZ0008000000192000145399")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("CZ65080000001920001453")] // Too short
    [InlineData("CZ650800000019200014539912")] // Too long
    [InlineData("CZ6508000000192000145XXX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateCzechIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = CzechRepublicIbanValidator.ValidateCzechIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly24Characters_ReturnsTrue()
    {
        // Czech IBANs must be exactly 24 characters
        var iban = "CZ6508000000192000145399"; // 24 chars

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(24);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("DE89370400440532013000")] // German IBAN
    public void IsValidIban_WithOtherCountryIbans_ReturnsFalse(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_VerifiesAllDigitsAfterCountryCode()
    {
        // Czech IBANs must be all digits after CZ
        var validIban = "CZ6508000000192000145399";

        // Act
        var result = _validator.Validate(validIban);

        // Assert
        result.IsValid.Should().BeTrue();

        // Verify structure
        var normalized = validIban.ToUpper();
        normalized[2..24].All(char.IsDigit).Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "CZ6508000000192000145399";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion

    #region Czech-Specific Modulo 11 Validation Tests

    [Fact]
    public void IsValidIban_WithInvalidMod11OnPrefix_ReturnsFalse()
    {
        // Czech IBAN with invalid Modulo 11 check on prefix should fail
        // This is caught by the Modulo 11 validation in the validator
        var invalidPrefixIban = "CZ6508000000192000145398"; // Changed last digit

        // Act
        var result = _validator.Validate(invalidPrefixIban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("CZ6508000000192000145399")] // Valid with prefix and account
    public void IsValidIban_WithValidMod11Checksum_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}


