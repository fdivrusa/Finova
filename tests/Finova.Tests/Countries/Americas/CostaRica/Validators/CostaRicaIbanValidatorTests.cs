using Finova.Countries.NorthAmerica.CostaRica.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.CostaRica.Validators;

public class CostaRicaIbanValidatorTests
{
    private readonly CostaRicaIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsCR() => _validator.CountryCode.Should().Be("CR");

    [Theory]
    [InlineData("CR05015202001026284066")]
    public void IsValidIban_WithValidCostaRicaIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("CR05 0152 0200 1026 2840 66")] // With spaces
    [InlineData("cr05015202001026284066")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("CR00015202001026284066")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("CR0501520200102628")] // Too short
    [InlineData("CR0501520200102628406699")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("CR05015202001026284066")]
    public void ValidateCostaRicaIban_WithValidIbans_ReturnsTrue(string iban)
        => CostaRicaIbanValidator.ValidateCostaRicaIban(iban).IsValid.Should().BeTrue();
}
