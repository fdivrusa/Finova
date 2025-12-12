using Finova.Countries.Europe.Germany.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany.Validators;

public class GermanyIbanValidatorTests
{
    private readonly GermanyIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsDE() => _validator.CountryCode.Should().Be("DE");

    [Theory]
    [InlineData("DE89370400440532013000")]
    [InlineData("DE44500105175407324931")]
    public void IsValidIban_WithValidGermanIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("DE89 3704 0044 0532 0130 00")] // With spaces
    [InlineData("de89370400440532013000")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("DE00370400440532013000")] // Wrong check digits
    [InlineData("FR1420041010050500013M02606")] // Wrong country
    [InlineData("DE893704004405320130")] // Too short
    [InlineData("DE89370400440532013000999")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("DE89370400440532013000")]
    public void ValidateGermanyIban_WithValidIbans_ReturnsTrue(string iban)
        => GermanyIbanValidator.ValidateGermanyIban(iban).IsValid.Should().BeTrue();
}



