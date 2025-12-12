using Finova.Countries.Europe.Montenegro.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Montenegro.Validators;

public class MontenegroIbanValidatorTests
{
    private readonly MontenegroIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsME()
    {
        _validator.CountryCode.Should().Be("ME");
    }

    [Theory]
    [InlineData("ME36500000000000000001")] // Generated valid IBAN
    public void IsValidIban_WithValidMontenegroIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ME36 5000 0000 0000 0000 01")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ME00100000000000000000")] // Wrong check digits
    [InlineData("FR78100000000000000000")] // Wrong country
    [InlineData("ME7810000000000000000")] // Too short
    [InlineData("ME781000000000000000001")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



