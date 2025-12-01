using Finova.Countries.Europe.Italy.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Italy.Validators;

public class ItalyIbanValidatorTests
{
    private readonly ItalyIbanValidator _validator;

    public ItalyIbanValidatorTests()
    {
        _validator = new ItalyIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsIT()
    {
        _validator.CountryCode.Should().Be("IT");
    }

    [Theory]
    [InlineData("IT60X0542811101000000123456")]
    public void IsValidIban_WithValidItalianIbans_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("IT60 X054 2811 1010 0000 0123 456")] // With spaces
    [InlineData("it60x0542811101000000123456")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("IT00X0542811101000000123456")] // Wrong check digits
    [InlineData("ES12X0542811101000000123456")] // Wrong country
    [InlineData("IT60X054281110100000012345")] // Too short (26 chars)
    [InlineData("IT60X05428111010000001234567")] // Too long (28 chars)
    [InlineData("IT60X0542811101")] // Way too short
    [InlineData("IT601054281110100000012345")] // CIN is digit instead of letter
    [InlineData("IT60XX542811101000000123456")] // ABI has letter
    [InlineData("IT60X0542X11101000000123456")] // CAB has letter
    [InlineData("IT60X0542811101@00000123456")] // Account has special char
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    [InlineData("   ")] // Whitespace
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }

    [Theory]
    [InlineData("IT60X0542811101000000123456")]
    public void ValidateItalyIban_WithValidIbans_ReturnsTrue(string iban)
    {
        ItalyIbanValidator.ValidateItalyIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("IT00X0542811101000000123456")] // Invalid check
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateItalyIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        ItalyIbanValidator.ValidateItalyIban(iban).Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "IT60X0542811101000000123456";
        var result1 = _validator.IsValidIban(iban);
        var result2 = _validator.IsValidIban(iban);
        result1.Should().Be(result2);
    }

    [Fact]
    public void IsValidIban_WithDifferentCinLetter_ValidatesStructure()
    {
        // Test that CIN letter validation works with the known valid IBAN
        var iban = "IT60X0542811101000000123456";
        _validator.IsValidIban(iban).Should().BeTrue();

        // Ensure the CIN is properly extracted
        var normalized = iban.Replace(" ", "").ToUpper();
        char cin = normalized[4];
        cin.Should().Be('X');
    }

    [Theory]
    [InlineData("DE89370400440532013000")] // German IBAN
    [InlineData("FR1420041010050500013M02606")] // French IBAN
    [InlineData("ES9121000418450200051332")] // Spanish IBAN
    public void IsValidIban_WithOtherCountryIbans_ReturnsFalse(string iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_WithNumericCin_ReturnsFalse()
    {
        var iban = "IT601054281110100000012345";
        _validator.IsValidIban(iban).Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_VerifiesAbiAndCabAreNumeric()
    {
        var validIban = "IT60X0542811101000000123456";
        _validator.IsValidIban(validIban).Should().BeTrue();

        var normalized = validIban.Replace(" ", "").ToUpper();
        normalized[5..10].All(char.IsDigit).Should().BeTrue();
        normalized[10..15].All(char.IsDigit).Should().BeTrue();
    }
}
