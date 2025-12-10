using Finova.Countries.Europe.Poland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Poland.Validators;

public class PolandIbanValidatorTests
{
    private readonly PolandIbanValidator _validator;

    public PolandIbanValidatorTests()
    {
        _validator = new PolandIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsPL()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("PL");
    }

    [Theory]
    [InlineData("PL61109010140000071219812874")] // PKO Bank example
    [InlineData("PL27114020040000300201355387")] // mBank example
    [InlineData("PL60102010260000042270201111")] // ING Bank example
    public void IsValidIban_WithValidPolishIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL61 1090 1014 0000 0712 1981 2874")] // With spaces
    [InlineData("pl61109010140000071219812874")] // Lowercase
    [InlineData("Pl61 1090 1014 0000 0712 1981 2874")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL00109010140000071219812874")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("PL6110901014000007121981287")] // Too short (27 chars)
    [InlineData("PL611090101400000712198128741")] // Too long (29 chars)
    [InlineData("PL61109010140000071219812XXX")] // Contains letters in account
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
    [InlineData("PL61109010140000071219812874")]
    [InlineData("PL27114020040000300201355387")]
    [InlineData("PL60102010260000042270201111")]
    public void ValidatePolandIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = PolandIbanValidator.ValidatePolandIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL61 1090 1014 0000 0712 1981 2874")] // With spaces
    [InlineData("pl61109010140000071219812874")] // Lowercase
    public void ValidatePolandIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = PolandIbanValidator.ValidatePolandIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL00109010140000071219812874")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("PL6110901014000007121981287")] // Too short
    [InlineData("PL611090101400000712198128741")] // Too long
    [InlineData("PL61109010140000071219812XXX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidatePolandIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = PolandIbanValidator.ValidatePolandIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly28Characters_ReturnsTrue()
    {
        // Polish IBANs must be exactly 28 characters
        var iban = "PL61109010140000071219812874"; // 28 chars

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
        // Polish IBANs must be all digits after PL
        var validIban = "PL61109010140000071219812874";

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
        var iban = "PL61109010140000071219812874";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion
}

