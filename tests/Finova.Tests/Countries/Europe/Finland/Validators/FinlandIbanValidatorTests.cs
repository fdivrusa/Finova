using Finova.Countries.Europe.Finland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Finland.Validators;

public class FinlandIbanValidatorTests
{
    private readonly FinlandIbanValidator _validator;

    public FinlandIbanValidatorTests()
    {
        _validator = new FinlandIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsFI()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("FI");
    }

    [Theory]
    [InlineData("FI2112345600000785")] // Nordea example
    [InlineData("FI1410093000123458")] // Aktia example
    public void IsValidIban_WithValidFinnishIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FI21 1234 5600 0007 85")] // With spaces
    [InlineData("fi2112345600000785")] // Lowercase
    [InlineData("Fi21 1234 5600 0007 85")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FI0012345600000785")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("FI21123456000007")] // Too short (16 chars)
    [InlineData("FI2112345600000785XX")] // Too long (20 chars)
    [InlineData("FI211234560000078X")] // Contains letters in numeric part
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
    [InlineData("FI2112345600000785")]
    [InlineData("FI1410093000123458")]
    public void ValidateFinlandIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = FinlandIbanValidator.ValidateFinlandIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FI21 1234 5600 0007 85")] // With spaces
    [InlineData("fi2112345600000785")] // Lowercase
    public void ValidateFinlandIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = FinlandIbanValidator.ValidateFinlandIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FI0012345600000785")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("FI21123456000007")] // Too short
    [InlineData("FI2112345600000785XX")] // Too long
    [InlineData("FI211234560000078X")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateFinlandIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = FinlandIbanValidator.ValidateFinlandIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly18Characters_ReturnsTrue()
    {
        // Finnish IBANs must be exactly 18 characters
        var iban = "FI2112345600000785"; // 18 chars

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(18);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("AT611904300234573201")] // Austrian IBAN
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
        // Finland IBANs must be all digits after FI
        var validIban = "FI2112345600000785";

        // Act
        var result = _validator.Validate(validIban);

        // Assert
        result.IsValid.Should().BeTrue();

        // Verify structure
        var normalized = validIban.ToUpper();
        normalized[2..18].All(char.IsDigit).Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "FI2112345600000785";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion
}


