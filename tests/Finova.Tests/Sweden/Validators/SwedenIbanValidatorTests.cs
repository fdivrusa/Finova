using Finova.Countries.Europe.Sweden.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Sweden.Validators;

public class SwedenIbanValidatorTests
{
    private readonly SwedenIbanValidator _validator;

    public SwedenIbanValidatorTests()
    {
        _validator = new SwedenIbanValidator();
    }

    #region Instance Method Tests

    [Fact]
    public void CountryCode_ReturnsSE()
    {
        // Act
        var result = _validator.CountryCode;

        // Assert
        result.Should().Be("SE");
    }

    [Theory]
    [InlineData("SE4550000000058398257466")] // SEB example
    [InlineData("SE3550000000054910000003")] // Nordea example
    [InlineData("SE6412000000012170145230")] // Swedbank example
    public void IsValidIban_WithValidSwedishIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SE45 5000 0000 0583 9825 7466")] // With spaces
    [InlineData("se4550000000058398257466")] // Lowercase
    [InlineData("Se45 5000 0000 0583 9825 7466")] // Mixed case with spaces
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SE0050000000058398257466")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("SE455000000005839825746")] // Too short (23 chars)
    [InlineData("SE45500000000583982574661")] // Too long (25 chars)
    [InlineData("SE4550000000058398257XXX")] // Contains letters in account
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
    [InlineData("SE4550000000058398257466")]
    [InlineData("SE3550000000054910000003")]
    [InlineData("SE6412000000012170145230")]
    public void ValidateSwedenIban_WithValidIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = SwedenIbanValidator.ValidateSwedenIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SE45 5000 0000 0583 9825 7466")] // With spaces
    [InlineData("se4550000000058398257466")] // Lowercase
    public void ValidateSwedenIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        // Act
        var result = SwedenIbanValidator.ValidateSwedenIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SE0050000000058398257466")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("SE455000000005839825746")] // Too short
    [InlineData("SE45500000000583982574661")] // Too long
    [InlineData("SE4550000000058398257XXX")] // Contains letters
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateSwedenIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        // Act
        var result = SwedenIbanValidator.ValidateSwedenIban(iban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_WithExactly24Characters_ReturnsTrue()
    {
        // Swedish IBANs must be exactly 24 characters
        var iban = "SE4550000000058398257466"; // 24 chars

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
        // Swedish IBANs must be all digits after SE
        var validIban = "SE4550000000058398257466";

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
        var iban = "SE4550000000058398257466";

        // Act
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    #endregion
}

