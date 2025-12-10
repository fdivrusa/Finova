using Finova.Countries.Europe.Bulgaria.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Bulgaria.Validators;

public class BulgariaIbanValidatorTests
{
    private readonly BulgariaIbanValidator _validator;

    public BulgariaIbanValidatorTests()
    {
        _validator = new BulgariaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsBG()
    {
        _validator.CountryCode.Should().Be("BG");
    }

    [Theory]
    [InlineData("BG19STSA93000123456789")]
    public void IsValidIban_WithValidBulgariaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BG19 STSA 9300 0123 4567 89")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BG00STSA93000123456789")] // Wrong check digits
    [InlineData("FR19STSA93000123456789")] // Wrong country
    [InlineData("BG19STSA9300012345678")] // Too short
    [InlineData("BG19STSA930001234567890")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}


