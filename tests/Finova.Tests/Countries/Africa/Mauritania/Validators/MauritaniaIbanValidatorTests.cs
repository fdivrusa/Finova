using Finova.Countries.Africa.Mauritania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Africa.Mauritania.Validators;

public class MauritaniaIbanValidatorTests
{
    private readonly MauritaniaIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsMR() => _validator.CountryCode.Should().Be("MR");

    [Theory]
    [InlineData("MR1300020001010000123456753")]
    public void IsValidIban_WithValidMauritaniaIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("MR13 0002 0001 0100 0012 3456 753")] // With spaces
    [InlineData("mr1300020001010000123456753")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("MR0000020001010000123456753")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("MR130002000101000012345")] // Too short
    [InlineData("MR130002000101000012345675399")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("MR1300020001010000123456753")]
    public void ValidateMauritaniaIban_WithValidIbans_ReturnsTrue(string iban)
        => MauritaniaIbanValidator.ValidateMauritaniaIban(iban).IsValid.Should().BeTrue();
}
