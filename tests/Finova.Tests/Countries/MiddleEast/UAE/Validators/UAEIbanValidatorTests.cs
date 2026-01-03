using Finova.Countries.MiddleEast.UAE.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.UAE.Validators;

public class UAEIbanValidatorTests
{
    private readonly UAEIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsAE() => _validator.CountryCode.Should().Be("AE");

    [Theory]
    [InlineData("AE070331234567890123456")]
    public void IsValidIban_WithValidUAEIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("AE07 0331 2345 6789 0123 456")] // With spaces
    [InlineData("ae070331234567890123456")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("AE000331234567890123456")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("AE07033123456789012")] // Too short
    [InlineData("AE07033123456789012345699")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("AE070331234567890123456")]
    public void ValidateUAEIban_WithValidIbans_ReturnsTrue(string iban)
        => UAEIbanValidator.ValidateUAEIban(iban).IsValid.Should().BeTrue();
}
