using Finova.Countries.Europe.Belarus.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belarus.Validators;

public class BelarusIbanValidatorTests
{
    private readonly BelarusIbanValidator _validator;

    public BelarusIbanValidatorTests()
    {
        _validator = new BelarusIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsBY()
    {
        _validator.CountryCode.Should().Be("BY");
    }

    [Fact]
    public void IsValidIban_WithValidBelarusIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "BY13NBRB3600900000002Z00AB00";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "BY13 NBRB 3600 9000 0000 2Z00 AB00";
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
    [InlineData("BY7786AKBB101000000029660")] // Too short
    [InlineData("BY7786AKBB10100000002966000")] // Too long
    [InlineData("XX7786AKBB1010000000296600")] // Wrong country code
    [InlineData("BY7786AKBB1010000000296601")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}

