using Finova.Countries.Europe.Liechtenstein.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Liechtenstein.Validators;

public class LiechtensteinIbanValidatorTests
{
    private readonly LiechtensteinIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsLI()
    {
        _validator.CountryCode.Should().Be("LI");
    }

    [Theory]
    [InlineData("LI2200000000000000001")] // Generated valid IBAN
    public void IsValidIban_WithValidLiechtensteinIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LI22 0000 0000 0000 0000 1")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LI0010000000000000000")] // Wrong check digits
    [InlineData("FR2410000000000000000")] // Wrong country
    [InlineData("LI241000000000000000")] // Too short
    [InlineData("LI24100000000000000001")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



