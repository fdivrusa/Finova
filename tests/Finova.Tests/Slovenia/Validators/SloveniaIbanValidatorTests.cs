using Finova.Countries.Europe.Slovenia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Slovenia.Validators;

public class SloveniaIbanValidatorTests
{
    private readonly SloveniaIbanValidator _validator;

    public SloveniaIbanValidatorTests()
    {
        _validator = new SloveniaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsSI()
    {
        _validator.CountryCode.Should().Be("SI");
    }

    [Theory]
    [InlineData("SI56192001234567892")]
    public void IsValidIban_WithValidSloveniaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SI56 1920 0123 4567 892")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SI00192001234567892")] // Wrong check digits
    [InlineData("FR56192001234567892")] // Wrong country
    [InlineData("SI5619200123456789")] // Too short
    [InlineData("SI561920012345678921")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}


