using Finova.Countries.Europe.Andorra.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Andorra.Validators;

public class AndorraIbanValidatorTests
{
    private readonly AndorraIbanValidator _validator;

    public AndorraIbanValidatorTests()
    {
        _validator = new AndorraIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsAD()
    {
        _validator.CountryCode.Should().Be("AD");
    }

    [Theory]
    [InlineData("AD1400080001001234567890")]
    public void IsValidIban_WithValidAndorraIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("AD14 0008 0001 0012 3456 7890")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("AD0000080001001234567890")] // Wrong check digits
    [InlineData("FR1400080001001234567890")] // Wrong country
    [InlineData("AD140008000100123456789")] // Too short
    [InlineData("AD14000800010012345678901")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }
}
