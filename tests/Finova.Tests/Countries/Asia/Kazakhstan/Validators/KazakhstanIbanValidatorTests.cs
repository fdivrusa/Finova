using Finova.Countries.Asia.Kazakhstan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Kazakhstan.Validators;

public class KazakhstanIbanValidatorTests
{
    private readonly KazakhstanIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsKZ() => _validator.CountryCode.Should().Be("KZ");

    [Theory]
    [InlineData("KZ86125KZT5004100100")]
    public void IsValidIban_WithValidKazakhstanIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("KZ86 125K ZT50 0410 0100")] // With spaces
    [InlineData("kz86125kzt5004100100")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("KZ00125KZT5004100100")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("KZ86125KZT50041")] // Too short
    [InlineData("KZ86125KZT500410010099")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("KZ86125KZT5004100100")]
    public void ValidateKazakhstanIban_WithValidIbans_ReturnsTrue(string iban)
        => KazakhstanIbanValidator.ValidateKazakhstanIban(iban).IsValid.Should().BeTrue();
}
