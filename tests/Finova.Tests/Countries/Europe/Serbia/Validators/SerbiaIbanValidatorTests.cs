using Finova.Countries.Europe.Serbia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Serbia.Validators;

public class SerbiaIbanValidatorTests
{
    private readonly SerbiaIbanValidator _validator;

    public SerbiaIbanValidatorTests()
    {
        _validator = new SerbiaIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsRS()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("RS");
    }

    [Theory]
    [InlineData("RS35260005601001611379")] // Banca Intesa example
    public void IsValidIban_WithValidSerbianIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RS35 2600 0560 1001 6113 79")] // With spaces
    [InlineData("rs35260005601001611379")] // Lowercase
    [InlineData("Rs35 2600 0560 1001 6113 79")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RS00260005601001611379")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("RS3526000560100161137")] // Too short (21 chars)
    [InlineData("RS352600056010016113791")] // Too long (23 chars)
    [InlineData("RS35260005601001611XXX")] // Contains letters in account
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
    [InlineData("RS35260005601001611379")]
    public void ValidateSerbiaIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = SerbiaIbanValidator.ValidateSerbiaIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RS35 2600 0560 1001 6113 79")] // With spaces
    [InlineData("rs35260005601001611379")] // Lowercase
    public void ValidateSerbiaIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = SerbiaIbanValidator.ValidateSerbiaIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RS00260005601001611379")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("RS3526000560100161137")] // Too short
    [InlineData("RS352600056010016113791")] // Too long
    [InlineData("RS35260005601001611XXX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateSerbiaIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = SerbiaIbanValidator.ValidateSerbiaIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly22Characters_ReturnsTrue()
    {
        // Serbian IBANs must be exactly 22 characters
        var iban = "RS35260005601001611379"; // 22 chars

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(22);
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
        // Serbian IBANs must be all digits after RS
        var validIban = "RS35260005601001611379";

        // Act
        var result = _validator.Validate(validIban);

        // Assert
        result.IsValid.Should().BeTrue();

        // Verify structure
        var normalized = validIban.ToUpper();
        normalized[2..22].All(char.IsDigit).Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "RS35260005601001611379";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion
}


