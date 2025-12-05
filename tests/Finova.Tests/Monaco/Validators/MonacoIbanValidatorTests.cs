using Finova.Countries.Europe.Monaco.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Monaco.Validators;

public class MonacoIbanValidatorTests
{
    private readonly MonacoIbanValidator _validator;

    public MonacoIbanValidatorTests()
    {
        _validator = new MonacoIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsMC()
    {
        _validator.CountryCode.Should().Be("MC");
    }

    [Theory]
    [InlineData("MC5810096180790123456789085")]
    public void IsValidIban_WithValidMonacoIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("MC58 1009 6180 7901 2345 6789 085")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.IsValidIban(iban).Should().BeTrue();
    }

    [Theory]
    [InlineData("MC0010096180790123456789085")] // Wrong check digits
    [InlineData("FR5810096180790123456789085")] // Wrong country
    [InlineData("MC581009618079012345678908")] // Too short
    [InlineData("MC58100961807901234567890851")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.IsValidIban(iban).Should().BeFalse();
    }
}
