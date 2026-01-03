using Finova.Countries.SouthAmerica.Brazil.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Brazil.Validators;

public class BrazilIbanValidatorTests
{
    private readonly BrazilIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsBR() => _validator.CountryCode.Should().Be("BR");

    [Theory]
    [InlineData("BR1800360305000010009795493C1")]
    public void IsValidIban_WithValidBrazilIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("BR18 0036 0305 0000 1000 9795 493C 1")] // With spaces
    [InlineData("br1800360305000010009795493c1")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("BR0000360305000010009795493C1")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("BR1800360305000010009795")] // Too short
    [InlineData("BR1800360305000010009795493C1999")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("BR1800360305000010009795493C1")]
    public void ValidateBrazilIban_WithValidIbans_ReturnsTrue(string iban)
        => BrazilIbanValidator.ValidateBrazilIban(iban).IsValid.Should().BeTrue();
}
