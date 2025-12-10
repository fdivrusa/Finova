using Finova.Countries.Europe.Albania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Albania.Validators;

public class AlbaniaIbanValidatorTests
{
    private readonly AlbaniaIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsAL()
    {
        _validator.CountryCode.Should().Be("AL");
    }

    [Theory]
    [InlineData("AL961001000C0000000000000001")] // Generated valid IBAN
    public void IsValidIban_WithValidAlbaniaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AL96 1001 000C 0000 0000 0000 0001")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AL00100100000000000000000001")] // Wrong check digits
    [InlineData("FR90100100000000000000000001")] // Wrong country
    [InlineData("AL9010010000000000000000000")] // Too short
    [InlineData("AL901001000000000000000000011")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}


