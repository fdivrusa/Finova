using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.DominicanRepublic.Validators;

public class DominicanRepublicIbanValidatorTests
{
    private readonly DominicanRepublicIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsDO() => _validator.CountryCode.Should().Be("DO");

    [Theory]
    [InlineData("DO22ACAU00000000000123456789")]
    public void IsValidIban_WithValidDominicanRepublicIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("DO22 ACAU 0000 0000 0001 2345 6789")] // With spaces
    [InlineData("do22acau00000000000123456789")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("DO00ACAU00000000000123456789")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("DO22ACAU000000000001234")] // Too short
    [InlineData("DO22ACAU0000000000012345678999")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("DO22ACAU00000000000123456789")]
    public void ValidateDominicanRepublicIban_WithValidIbans_ReturnsTrue(string iban)
        => DominicanRepublicIbanValidator.ValidateDominicanRepublicIban(iban).IsValid.Should().BeTrue();
}
