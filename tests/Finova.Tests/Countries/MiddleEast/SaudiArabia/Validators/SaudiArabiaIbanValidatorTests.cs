using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.SaudiArabia.Validators;

public class SaudiArabiaIbanValidatorTests
{
    private readonly SaudiArabiaIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsSA() => _validator.CountryCode.Should().Be("SA");

    [Theory]
    [InlineData("SA0380000000608010167519")]
    public void IsValidIban_WithValidSaudiArabiaIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("SA03 8000 0000 6080 1016 7519")] // With spaces
    [InlineData("sa0380000000608010167519")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("SA0080000000608010167519")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("SA038000000060801016")] // Too short
    [InlineData("SA038000000060801016751999")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("SA0380000000608010167519")]
    public void ValidateSaudiArabiaIban_WithValidIbans_ReturnsTrue(string iban)
        => SaudiArabiaIbanValidator.ValidateSaudiArabiaIban(iban).IsValid.Should().BeTrue();
}
