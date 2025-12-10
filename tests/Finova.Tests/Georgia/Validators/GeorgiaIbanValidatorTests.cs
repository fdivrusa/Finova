using Finova.Countries.Europe.Georgia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Georgia.Validators;

public class GeorgiaIbanValidatorTests
{
    private readonly GeorgiaIbanValidator _validator;

    public GeorgiaIbanValidatorTests()
    {
        _validator = new GeorgiaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsGE()
    {
        _validator.CountryCode.Should().Be("GE");
    }

    [Fact]
    public void IsValidIban_WithValidGeorgiaIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "GE6329NB00000001019049";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "GE63 29NB 0000 0001 0190 49";
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
    [InlineData("GE6329NB0000000101904")] // Too short
    [InlineData("GE6329NB000000010190499")] // Too long
    [InlineData("XX6329NB00000001019049")] // Wrong country code
    [InlineData("GE6329NB00000001019048")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}
