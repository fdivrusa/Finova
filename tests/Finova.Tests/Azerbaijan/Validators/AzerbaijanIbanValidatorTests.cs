using Finova.Countries.Europe.Azerbaijan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Azerbaijan.Validators;

public class AzerbaijanIbanValidatorTests
{
    private readonly AzerbaijanIbanValidator _validator;

    public AzerbaijanIbanValidatorTests()
    {
        _validator = new AzerbaijanIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsAZ()
    {
        _validator.CountryCode.Should().Be("AZ");
    }

    [Fact]
    public void IsValidIban_WithValidAzerbaijanIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "AZ21NABZ00000000137010001944";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "AZ21 NABZ 0000 0000 1370 1000 1944";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void IsValidIban_WithNullOrEmpty_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("AZ9221NABZ00000000137010001")] // Too short
    [InlineData("AZ9221NABZ0000000013701000199")] // Too long
    [InlineData("XX9221NABZ000000001370100019")] // Wrong country code
    [InlineData("AZ9221NABZ000000001370100018")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}
