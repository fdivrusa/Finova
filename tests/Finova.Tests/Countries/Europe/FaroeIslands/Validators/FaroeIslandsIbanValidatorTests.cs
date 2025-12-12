using Finova.Countries.Europe.FaroeIslands.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.FaroeIslands.Validators;

public class FaroeIslandsIbanValidatorTests
{
    private readonly FaroeIslandsIbanValidator _validator;

    public FaroeIslandsIbanValidatorTests()
    {
        _validator = new FaroeIslandsIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsFO()
    {
        _validator.CountryCode.Should().Be("FO");
    }

    [Fact]
    public void IsValidIban_WithValidFaroeIslandsIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "FO7162646000016316";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "FO71 6264 6000 0163 16";
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
    [InlineData("FO716264600001631")] // Too short
    [InlineData("FO71626460000163166")] // Too long
    [InlineData("XX7162646000016316")] // Wrong country code
    [InlineData("FO7162646000016317")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}

