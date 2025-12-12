using Finova.Countries.Europe.Kosovo.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Kosovo.Validators;

public class KosovoIbanValidatorTests
{
    private readonly KosovoIbanValidator _validator;

    public KosovoIbanValidatorTests()
    {
        _validator = new KosovoIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsXK()
    {
        _validator.CountryCode.Should().Be("XK");
    }

    [Fact]
    public void IsValidIban_WithValidKosovoIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "XK030512120123456789";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "XK03 0512 1201 2345 6789";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void IsValidIban_WithNullOrEmpty_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("XK05051212012345678")] // Too short
    [InlineData("XK0505121201234567890")] // Too long
    [InlineData("XX050512120123456789")] // Wrong country code
    [InlineData("XK050512120123456788")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}

