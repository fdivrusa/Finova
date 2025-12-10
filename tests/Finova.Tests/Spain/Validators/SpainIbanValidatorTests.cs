using Finova.Countries.Europe.Spain.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Spain.Validators;

public class SpainIbanValidatorTests
{
    private readonly SpainIbanValidator _validator;

    public SpainIbanValidatorTests()
    {
        _validator = new SpainIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsES()
    {
        _validator.CountryCode.Should().Be("ES");
    }

    [Theory]
    [InlineData("ES9121000418450200051332")]
    [InlineData("ES6621000418401234567891")]
    public void IsValidIban_WithValidSpanishIbans_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ES91 2100 0418 4502 0005 1332")] // With spaces
    [InlineData("es9121000418450200051332")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ES0021000418450200051332")] // Wrong check digits
    [InlineData("IT60X0542811101000000123456")] // Wrong country
    [InlineData("ES912100041845020005133")] // Too short (23 chars)
    [InlineData("ES91210004184502000513322")] // Too long (25 chars)
    [InlineData("ES9121000418")] // Way too short
    [InlineData("ES91X1000418450200051332")] // Letter in Entidad
    [InlineData("ES912100X418450200051332")] // Letter in Oficina
    [InlineData("ES9121000418X50200051332")] // Letter in DC
    [InlineData("ES912100041845020005133X")] // Letter in Cuenta
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    [InlineData("   ")] // Whitespace
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("ES9121000418450200051332")]
    [InlineData("ES6621000418401234567891")]
    public void ValidateSpainIban_WithValidIbans_ReturnsTrue(string iban)
    {
        SpainIbanValidator.ValidateSpainIban(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ES0021000418450200051332")] // Invalid check
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateSpainIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        SpainIbanValidator.ValidateSpainIban(iban).IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "ES9121000418450200051332";
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);
        result1.Should().BeEquivalentTo(result2);
    }

    [Theory]
    [InlineData("ES9121000418450200051332")] // Entidad: 2100
    [InlineData("ES6621000418401234567891")] // Entidad: 2100
    public void IsValidIban_WithDifferentBanks_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("DE89370400440532013000")] // German IBAN
    [InlineData("FR1420041010050500013M02606")] // French IBAN
    [InlineData("IT60X0542811101000000123456")] // Italian IBAN
    public void IsValidIban_WithOtherCountryIbans_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_WithOnlyDigitsAfterCountryCode_ReturnsTrue()
    {
        var iban = "ES9121000418450200051332";
        _validator.Validate(iban).IsValid.Should().BeTrue();

        // Verify it's all digits after ES
        for (int i = 2; i < iban.Length; i++)
        {
            char.IsDigit(iban[i]).Should().BeTrue();
        }
    }

    [Theory]
    [InlineData("ES9100000000000000000000")] // Edge case - all zeros after check
    [InlineData("ES9199999999999999999999")] // Edge case - all nines
    public void IsValidIban_WithEdgeCases_ValidatesCorrectly(string iban)
    {
        // These should validate structure but may fail mod97 check
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeFalse(); // These are structurally valid but fail checksum
    }
}


