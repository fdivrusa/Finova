using Finova.Countries.Europe.Iceland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Iceland.Validators;

public class IcelandIbanValidatorTests
{
    private readonly IcelandIbanValidator _validator;

    public IcelandIbanValidatorTests()
    {
        _validator = new IcelandIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsIS()
    {
        _validator.CountryCode.Should().Be("IS");
    }

    [Theory]
    [InlineData("IS750001121234563108962099")]
    public void IsValidIban_WithValidIcelandIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IS75 0001 1212 3456 3108 9620 99")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IS000001121234563108962099")] // Wrong check digits
    [InlineData("FR750001121234563108962099")] // Wrong country
    [InlineData("IS75000112123456310896209")] // Too short
    [InlineData("IS7500011212345631089620991")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



