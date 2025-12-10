using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.BosniaAndHerzegovina.Validators;

public class BosniaAndHerzegovinaIbanValidatorTests
{
    private readonly BosniaAndHerzegovinaIbanValidator _validator;

    public BosniaAndHerzegovinaIbanValidatorTests()
    {
        _validator = new BosniaAndHerzegovinaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsBA()
    {
        _validator.CountryCode.Should().Be("BA");
    }

    [Fact]
    public void IsValidIban_WithValidBosniaAndHerzegovinaIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "BA383933858048002112";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "BA38 3933 8580 4800 2112";
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
    [InlineData("BA39393385804800211")] // Too short
    [InlineData("BA3939338580480021123")] // Too long
    [InlineData("XX393933858048002112")] // Wrong country code
    [InlineData("BA393933858048002113")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}
