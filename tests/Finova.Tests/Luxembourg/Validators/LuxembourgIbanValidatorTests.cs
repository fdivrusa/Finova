using Finova.Countries.Europe.Luxembourg.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Luxembourg.Validators;

public class LuxembourgIbanValidatorTests
{
    private readonly LuxembourgIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsLU() => _validator.CountryCode.Should().Be("LU");

    [Theory]
    [InlineData("LU280019400644750000")]
    [InlineData("LU120010001234567891")]
    public void IsValidIban_WithValidLuxembourgIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("LU28 0019 4006 4475 0000")] // With spaces
    [InlineData("lu280019400644750000")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("LU000019400644750000")] // Wrong check digits
    [InlineData("BE68539007547034")] // Wrong country
    [InlineData("LU28001940064475")] // Too short
    [InlineData("LU28001940064475000099")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("LU280019400644750000")]
    public void ValidateLuxembourgIban_WithValidIbans_ReturnsTrue(string iban)
        => LuxembourgIbanValidator.ValidateLuxembourgIban(iban).IsValid.Should().BeTrue();
}


