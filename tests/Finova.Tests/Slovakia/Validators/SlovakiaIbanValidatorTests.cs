using Finova.Countries.Europe.Slovakia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Slovakia.Validators;

public class SlovakiaIbanValidatorTests
{
    private readonly SlovakiaIbanValidator _validator;

    public SlovakiaIbanValidatorTests()
    {
        _validator = new SlovakiaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsSK()
    {
        _validator.CountryCode.Should().Be("SK");
    }

    [Theory]
    [InlineData("SK8975000000000012345671")]
    public void IsValidIban_WithValidSlovakiaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SK89 7500 0000 0000 1234 5671")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SK0075000000000012345671")] // Wrong check digits
    [InlineData("FR8975000000000012345671")] // Wrong country
    [InlineData("SK897500000000001234567")] // Too short
    [InlineData("SK89750000000000123456711")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}


