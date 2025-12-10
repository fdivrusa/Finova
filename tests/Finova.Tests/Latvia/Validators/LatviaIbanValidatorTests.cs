using Finova.Countries.Europe.Latvia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Latvia.Validators;

public class LatviaIbanValidatorTests
{
    private readonly LatviaIbanValidator _validator;

    public LatviaIbanValidatorTests()
    {
        _validator = new LatviaIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsLV()
    {
        _validator.CountryCode.Should().Be("LV");
    }

    [Theory]
    [InlineData("LV97HABA0012345678910")]
    public void IsValidIban_WithValidLatviaIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LV97 HABA 0012 3456 7891 0")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LV98HABA0012345678910")] // Wrong check digits
    [InlineData("FR97HABA0012345678910")] // Wrong country
    [InlineData("LV97HABA001234567891")] // Too short
    [InlineData("LV97HABA00123456789101")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}


