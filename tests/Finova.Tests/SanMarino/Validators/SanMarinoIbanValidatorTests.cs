using Finova.Countries.Europe.SanMarino.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.SanMarino.Validators;

public class SanMarinoIbanValidatorTests
{
    private readonly SanMarinoIbanValidator _validator;

    public SanMarinoIbanValidatorTests()
    {
        _validator = new SanMarinoIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsSM()
    {
        _validator.CountryCode.Should().Be("SM");
    }

    [Theory]
    [InlineData("SM76P0854009812123456789123")]
    public void IsValidIban_WithValidSanMarinoIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("SM76 P085 4009 8121 2345 6789 123")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("SM00P0854009812123456789123")] // Wrong check digits
    [InlineData("FR76P0854009812123456789123")] // Wrong country
    [InlineData("SM76P085400981212345678912")] // Too short
    [InlineData("SM76P08540098121234567891231")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }
}
