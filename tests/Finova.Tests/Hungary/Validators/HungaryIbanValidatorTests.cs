using Finova.Countries.Europe.Hungary.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Hungary.Validators;

public class HungaryIbanValidatorTests
{
    private readonly HungaryIbanValidator _validator;

    public HungaryIbanValidatorTests()
    {
        _validator = new HungaryIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsHU()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("HU");
    }

    [Theory]
    [InlineData("HU42117730161111101800000000")] // OTP Bank example
    public void IsValidIban_WithValidHungarianIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HU42 1177 3016 1111 1018 0000 0000")] // With spaces
    [InlineData("hu42117730161111101800000000")] // Lowercase
    [InlineData("Hu42 1177 3016 1111 1018 0000 0000")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HU00117730161111101800000000")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("HU4211773016111110180000000")] // Too short (27 chars)
    [InlineData("HU421177301611111018000000001")] // Too long (29 chars)
    [InlineData("HU42117730161111101800000XXX")] // Contains letters in account
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
    [InlineData("HU42117730161111101800000000")]
    public void ValidateHungaryIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = HungaryIbanValidator.ValidateHungaryIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HU42 1177 3016 1111 1018 0000 0000")] // With spaces
    [InlineData("hu42117730161111101800000000")] // Lowercase
    public void ValidateHungaryIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = HungaryIbanValidator.ValidateHungaryIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HU00117730161111101800000000")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("HU4211773016111110180000000")] // Too short
    [InlineData("HU421177301611111018000000001")] // Too long
    [InlineData("HU42117730161111101800000XXX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateHungaryIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = HungaryIbanValidator.ValidateHungaryIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly28Characters_ReturnsTrue()
    {
        // Hungarian IBANs must be exactly 28 characters
        var iban = "HU42117730161111101800000000"; // 28 chars

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(28);
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
        // Hungarian IBANs must be all digits after HU
        var validIban = "HU42117730161111101800000000";

        // Act
        var result = _validator.Validate(validIban);

        // Assert
        result.IsValid.Should().BeTrue();

        // Verify structure
        var normalized = validIban.ToUpper();
        normalized[2..28].All(char.IsDigit).Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "HU42117730161111101800000000";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion
}

