using Finova.Countries.Europe.Vatican.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Vatican.Validators;

public class VaticanIbanValidatorTests
{
    private readonly VaticanIbanValidator _validator;

    public VaticanIbanValidatorTests()
    {
        _validator = new VaticanIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsVA()
    {
        _validator.CountryCode.Should().Be("VA");
    }

    [Theory]
    [InlineData("VA59001123000012345678")]
    public void IsValidIban_WithValidVaticanIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("VA59 0011 2300 0012 3456 78")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("VA00001123000012345678")] // Wrong check digits
    [InlineData("FR59001123000012345678")] // Wrong country
    [InlineData("VA5900112300001234567")] // Too short
    [InlineData("VA590011230000123456789")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }
}
