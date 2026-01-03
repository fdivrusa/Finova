using Finova.Countries.Asia.TimorLeste.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.TimorLeste.Validators;

public class TimorLesteIbanValidatorTests
{
    private readonly TimorLesteIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsTL() => _validator.CountryCode.Should().Be("TL");

    [Theory]
    [InlineData("TL380080012345678910157")]
    public void IsValidIban_WithValidTimorLesteIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("TL38 0080 0123 4567 8910 157")] // With spaces
    [InlineData("tl380080012345678910157")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("TL000080012345678910157")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("TL3800800123456789")] // Too short
    [InlineData("TL38008001234567891015799")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("TL380080012345678910157")]
    public void ValidateTimorLesteIban_WithValidIbans_ReturnsTrue(string iban)
        => TimorLesteIbanValidator.ValidateTimorLesteIban(iban).IsValid.Should().BeTrue();
}
