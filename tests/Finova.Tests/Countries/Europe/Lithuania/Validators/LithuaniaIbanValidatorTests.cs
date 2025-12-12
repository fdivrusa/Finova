using Finova.Countries.Europe.Lithuania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Lithuania.Validators;

public class LithuaniaIbanValidatorTests
{
    private readonly LithuaniaIbanValidator _validator;

    public LithuaniaIbanValidatorTests()
    {
        _validator = new LithuaniaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsLT()
    {
        _validator.CountryCode.Should().Be("LT");
    }

    [Theory]
    [InlineData("LT601010012345678901")]
    public void IsValidIban_WithValidLithuaniaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LT60 1010 0123 4567 8901")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LT001010012345678901")] // Wrong check digits
    [InlineData("FR601010012345678901")] // Wrong country
    [InlineData("LT60101001234567890")] // Too short
    [InlineData("LT6010100123456789011")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



