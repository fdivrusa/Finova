using Finova.Countries.Europe.Croatia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Croatia.Validators;

public class CroatiaIbanValidatorTests
{
    private readonly CroatiaIbanValidator _validator;

    public CroatiaIbanValidatorTests()
    {
        _validator = new CroatiaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsHR()
    {
        _validator.CountryCode.Should().Be("HR");
    }

    [Theory]
    [InlineData("HR1723600001101234565")]
    public void IsValidIban_WithValidCroatiaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HR17 2360 0001 1012 3456 5")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HR0023600001101234565")] // Wrong check digits
    [InlineData("FR1723600001101234565")] // Wrong country
    [InlineData("HR172360000110123456")] // Too short
    [InlineData("HR17236000011012345651")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



