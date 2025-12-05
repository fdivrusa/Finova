using Finova.Countries.Europe.Moldova.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Moldova.Validators;

public class MoldovaIbanValidatorTests
{
    private readonly MoldovaIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsMD()
    {
        _validator.CountryCode.Should().Be("MD");
    }

    [Theory]
    [InlineData("MD76AA100000000000000001")] // Generated valid IBAN
    public void IsValidIban_WithValidMoldovaIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("MD76 AA10 0000 0000 0000 0001")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("MD0010000000000000000000")] // Wrong check digits
    [InlineData("FR3610000000000000000000")] // Wrong country
    [InlineData("MD361000000000000000000")] // Too short
    [InlineData("MD36100000000000000000001")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }
}
