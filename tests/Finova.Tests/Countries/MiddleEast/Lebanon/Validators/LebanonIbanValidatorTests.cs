using Finova.Countries.MiddleEast.Lebanon.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Lebanon.Validators;

public class LebanonIbanValidatorTests
{
    private readonly LebanonIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsLB() => _validator.CountryCode.Should().Be("LB");

    [Theory]
    [InlineData("LB62099900000001001901229114")]
    public void IsValidIban_WithValidLebanonIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("LB62 0999 0000 0001 0019 0122 9114")] // With spaces
    [InlineData("lb62099900000001001901229114")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("LB00099900000001001901229114")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("LB6209990000000100190122")] // Too short
    [InlineData("LB6209990000000100190122911499")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("LB62099900000001001901229114")]
    public void ValidateLebanonIban_WithValidIbans_ReturnsTrue(string iban)
        => LebanonIbanValidator.ValidateLebanonIban(iban).IsValid.Should().BeTrue();
}
