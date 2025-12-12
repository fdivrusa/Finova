using Finova.Countries.Europe.NorthMacedonia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.NorthMacedonia.Validators;

public class NorthMacedoniaIbanValidatorTests
{
    private readonly NorthMacedoniaIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsMK()
    {
        _validator.CountryCode.Should().Be("MK");
    }

    [Theory]
    [InlineData("MK31100000000000001")] // Generated valid IBAN
    public void IsValidIban_WithValidNorthMacedoniaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("MK31 1000 0000 0000 001")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("MK00100000000000000")] // Wrong check digits
    [InlineData("FR80100000000000000")] // Wrong country
    [InlineData("MK8010000000000000")] // Too short
    [InlineData("MK801000000000000001")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



