using Finova.Countries.Europe.Turkey.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Turkey.Validators;

public class TurkeyIbanValidatorTests
{
    private readonly TurkeyIbanValidator _validator;

    public TurkeyIbanValidatorTests()
    {
        _validator = new TurkeyIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsTR()
    {
        _validator.CountryCode.Should().Be("TR");
    }

    [Fact]
    public void IsValidIban_WithValidTurkeyIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "TR943300061005197864578413";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "TR94 3300 0610 0519 7864 5784 13";
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
    [InlineData("TR94330006100519786457841")] // Too short
    [InlineData("TR9433000610051978645784133")] // Too long
    [InlineData("XX943300061005197864578413")] // Wrong country code
    [InlineData("TR943300061005197864578414")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}
