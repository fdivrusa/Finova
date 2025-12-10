using Finova.Countries.Europe.Greenland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Greenland.Validators;

public class GreenlandIbanValidatorTests
{
    private readonly GreenlandIbanValidator _validator;

    public GreenlandIbanValidatorTests()
    {
        _validator = new GreenlandIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsGL()
    {
        _validator.CountryCode.Should().Be("GL");
    }

    [Fact]
    public void IsValidIban_WithValidGreenlandIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "GL1589647101234567";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "GL15 8964 7101 2345 67";
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
    [InlineData("GL158964710123456")] // Too short
    [InlineData("GL15896471012345678")] // Too long
    [InlineData("XX1589647101234567")] // Wrong country code
    [InlineData("GL1589647101234568")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}
