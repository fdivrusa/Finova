using Finova.Countries.Europe.Estonia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Estonia.Validators;

public class EstoniaIbanValidatorTests
{
    private readonly EstoniaIbanValidator _validator;

    public EstoniaIbanValidatorTests()
    {
        _validator = new EstoniaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsEE()
    {
        _validator.CountryCode.Should().Be("EE");
    }

    [Theory]
    [InlineData("EE201000001020145686")]
    public void IsValidIban_WithValidEstoniaIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("EE20 1000 0010 2014 5686")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("EE001000001020145686")] // Wrong check digits
    [InlineData("FR201000001020145686")] // Wrong country
    [InlineData("EE20100000102014568")] // Too short
    [InlineData("EE2010000010201456861")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }
}
