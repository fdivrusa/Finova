using Finova.Countries.Europe.Malta.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Malta.Validators;

public class MaltaIbanValidatorTests
{
    private readonly MaltaIbanValidator _validator;

    public MaltaIbanValidatorTests()
    {
        _validator = new MaltaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsMT()
    {
        _validator.CountryCode.Should().Be("MT");
    }

    [Theory]
    [InlineData("MT31MALT01100000000000000000123")]
    public void IsValidIban_WithValidMaltaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("MT31 MALT 0110 0000 0000 0000 0000 123")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("MT00MALT01100000000000000000123")] // Wrong check digits
    [InlineData("FR31MALT01100000000000000000123")] // Wrong country
    [InlineData("MT31MALT0110000000000000000012")] // Too short
    [InlineData("MT31MALT011000000000000000001231")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



