using Finova.Countries.MiddleEast.Bahrain.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Bahrain.Validators;

public class BahrainIbanValidatorTests
{
    private readonly BahrainIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsBH() => _validator.CountryCode.Should().Be("BH");

    [Theory]
    [InlineData("BH67BMAG00001299123456")]
    public void IsValidIban_WithValidBahrainIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("BH67 BMAG 0000 1299 1234 56")] // With spaces
    [InlineData("bh67bmag00001299123456")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("BH00BMAG00001299123456")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("BH67BMAG000012991234")] // Too short
    [InlineData("BH67BMAG000012991234567890")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("BH67BMAG00001299123456")]
    public void ValidateBahrainIban_WithValidIbans_ReturnsTrue(string iban)
        => BahrainIbanValidator.ValidateBahrainIban(iban).IsValid.Should().BeTrue();
}
