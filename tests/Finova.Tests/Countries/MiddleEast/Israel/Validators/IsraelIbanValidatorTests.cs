using Finova.Countries.MiddleEast.Israel.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Israel.Validators;

public class IsraelIbanValidatorTests
{
    private readonly IsraelIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsIL() => _validator.CountryCode.Should().Be("IL");

    [Theory]
    [InlineData("IL620108000000099999999")]
    public void IsValidIban_WithValidIsraelIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("IL62 0108 0000 0009 9999 999")] // With spaces
    [InlineData("il620108000000099999999")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("IL000108000000099999999")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("IL62010800000009999")] // Too short
    [InlineData("IL620108000000099999999999")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("IL620108000000099999999")]
    public void ValidateIsraelIban_WithValidIbans_ReturnsTrue(string iban)
        => IsraelIbanValidator.ValidateIsraelIban(iban).IsValid.Should().BeTrue();
}
