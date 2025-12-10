using Finova.Countries.Europe.Cyprus.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Cyprus.Validators;

public class CyprusIbanValidatorTests
{
    private readonly CyprusIbanValidator _validator;

    public CyprusIbanValidatorTests()
    {
        _validator = new CyprusIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsCY()
    {
        _validator.CountryCode.Should().Be("CY");
    }

    [Theory]
    [InlineData("CY21002001950000357001234567")]
    public void IsValidIban_WithValidCyprusIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CY21 0020 0195 0000 3570 0123 4567")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CY00002001950000357001234567")] // Wrong check digits
    [InlineData("FR21002001950000357001234567")] // Wrong country
    [InlineData("CY2100200195000035700123456")] // Too short
    [InlineData("CY210020019500003570012345671")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}


