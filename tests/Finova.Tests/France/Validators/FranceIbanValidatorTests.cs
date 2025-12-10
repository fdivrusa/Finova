using Finova.Countries.Europe.France.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.France.Validators;

public class FranceIbanValidatorTests
{
    private readonly FranceIbanValidator _validator;

    public FranceIbanValidatorTests()
    {
        _validator = new FranceIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsFR()
    {
        _validator.CountryCode.Should().Be("FR");
    }

    [Theory]
    [InlineData("FR1420041010050500013M02606")]
    [InlineData("FR7630006000011234567890189")]
    public void IsValidIban_WithValidFrenchIbans_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FR14 2004 1010 0505 0001 3M02 606")] // With spaces
    [InlineData("fr1420041010050500013m02606")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FR0020041010050500013M02606")] // Wrong check digits
    [InlineData("BE68539007547034")] // Wrong country
    [InlineData("FR142004101005050001")] // Too short
    [InlineData("FR142004101005050001333M02606999")] // Too long
    [InlineData("FR14X0041010050500013M02606")] // Letter in bank code
    [InlineData("FR142004X010050500013M02606")] // Letter in branch code
    [InlineData("FR14200410100505000133026X")] // Letter in RIB key
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("FR1420041010050500013M02606")]
    [InlineData("FR7630006000011234567890189")]
    public void ValidateFranceIban_WithValidIbans_ReturnsTrue(string iban)
    {
        FranceIbanValidator.ValidateFranceIban(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FR0020041010050500013M02606")]
    [InlineData("")]
    [InlineData(null)]
    public void ValidateFranceIban_WithInvalidIbans_ReturnsFalse(string? iban)
    {
        FranceIbanValidator.ValidateFranceIban(iban).IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "FR1420041010050500013M02606";
        var result1 = _validator.Validate(iban);
        var result2 = _validator.Validate(iban);
        result1.Should().BeEquivalentTo(result2);
    }
}


