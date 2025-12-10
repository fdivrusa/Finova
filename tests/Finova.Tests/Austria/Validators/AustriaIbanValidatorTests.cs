using Finova.Countries.Europe.Austria.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Austria.Validators;

public class AustriaIbanValidatorTests
{
    private readonly AustriaIbanValidator _validator;

    public AustriaIbanValidatorTests()
    {
        _validator = new AustriaIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsAT()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("AT");
    }

    [Theory]
    [InlineData("AT611904300234573201")] // Erste Bank example
    [InlineData("AT483200000012345864")] // Raiffeisen example
    [InlineData("AT022050302101023600")] // Bank Austria example
    public void IsValidIban_WithValidAustrianIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AT61 1904 3002 3457 3201")] // With spaces
    [InlineData("at611904300234573201")] // Lowercase
    [InlineData("At61 1904 3002 3457 3201")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AT001904300234573201")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("AT6119043002345732")] // Too short (18 chars)
    [InlineData("AT61190430023457320112")] // Too long (22 chars)
    [InlineData("AT6119043002345732XX")] // Contains letters in account
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
    [InlineData("AT611904300234573201")]
    [InlineData("AT483200000012345864")]
    [InlineData("AT022050302101023600")]
    public void ValidateAustriaIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = AustriaIbanValidator.ValidateAustriaIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AT61 1904 3002 3457 3201")] // With spaces
    [InlineData("at611904300234573201")] // Lowercase
    public void ValidateAustriaIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = AustriaIbanValidator.ValidateAustriaIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AT001904300234573201")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("AT6119043002345732")] // Too short
    [InlineData("AT61190430023457320112")] // Too long
    [InlineData("AT6119043002345732XX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateAustriaIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = AustriaIbanValidator.ValidateAustriaIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly20Characters_ReturnsTrue()
    {
        // Austrian IBANs must be exactly 20 characters
        var iban = "AT611904300234573201"; // 20 chars

        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
        iban.Replace(" ", "").Length.Should().Be(20);
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
        // Austria IBANs must be all digits after AT
        var validIban = "AT611904300234573201";

        // Act
        var result = _validator.Validate(validIban);

        // Assert
        result.IsValid.Should().BeTrue();

        // Verify structure
        var normalized = validIban.ToUpper();
        normalized[2..20].All(char.IsDigit).Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "AT611904300234573201";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion
}

