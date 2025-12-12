using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Validators;

public class BelgiumIbanValidatorTests
{
    private readonly BelgiumIbanValidator _validator;

    public BelgiumIbanValidatorTests()
    {
        _validator = new BelgiumIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsBE()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("BE");
    }

    [Theory]
    [InlineData("BE68539007547034")]
    [InlineData("BE71096123456769")]
    public void IsValidIban_WithValidBelgianIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034")] // With spaces
    [InlineData("be68539007547034")] // Lowercase
    [InlineData("Be68 5390 0754 7034")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE00539007547034")] // Wrong check digits
    [InlineData("NL91ABNA0417164300")] // Wrong country
    [InlineData("BE685390075470")] // Too short (14 chars)
    [InlineData("BE6853900754703412")] // Too long (18 chars)
    [InlineData("BE6853900754703X")] // Contains letter
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
    [InlineData("BE68539007547034")]
    [InlineData("BE71096123456769")]
    public void ValidateBelgianIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034")] // With spaces
    [InlineData("be68539007547034")] // Lowercase
    public void ValidateBelgianIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE00539007547034")] // Wrong check digits
    [InlineData("NL91ABNA0417164300")] // Wrong country
    [InlineData("BE685390075470")] // Too short
    [InlineData("BE6853900754703412")] // Too long
    [InlineData("BE6853900754703X")] // Contains letter
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateBelgianIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Theory]
    [InlineData("BE68539007547034")] // Valid IBAN with all digits
    [InlineData("BE71096123456769")] // Valid IBAN with zeros
    public void ValidateBelgianIban_WithValidStructure_ReturnsTrue(string iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE68A39007547034")] // Letter in bank code
    [InlineData("BE6853900754703B")] // Letter in account number
    [InlineData("BE68539X07547034")] // Letter in middle
    public void ValidateBelgianIban_WithLettersInNumericPart_ReturnsFalse(string iban)
    {
        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ValidateBelgianIban_WithAllZeros_ReturnsFalse()
    {
        // Arrange
        var iban = "BE00000000000000";

        // Act
        var result = BelgiumIbanValidator.ValidateBelgiumIban(iban);

        // Assert
        result.IsValid.Should().BeFalse("Checksum should be invalid");
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "BE68539007547034";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);
        var result3 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
        result2.Should().BeEquivalentTo(result3);
    }

    #endregion
}


