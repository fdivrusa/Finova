using Finova.Countries.Europe.Switzerland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Switzerland.Validators;

public class SwitzerlandIbanValidatorTests
{
    private readonly SwitzerlandIbanValidator _validator;

    public SwitzerlandIbanValidatorTests()
    {
        _validator = new SwitzerlandIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsCH()
    {
        _validator.CountryCode.Should().Be("CH");
    }

    [Theory]
    [InlineData("CH5604835012345678009")]
    public void IsValidIban_WithValidSwitzerlandIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CH56 0483 5012 3456 7800 9")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CH0004835012345678009")] // Wrong check digits
    [InlineData("FR5604835012345678009")] // Wrong country
    [InlineData("CH560483501234567800")] // Too short
    [InlineData("CH56048350123456780091")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



