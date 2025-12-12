using Finova.Countries.Europe.Gibraltar.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Gibraltar.Validators;

public class GibraltarIbanValidatorTests
{
    private readonly GibraltarIbanValidator _validator;

    public GibraltarIbanValidatorTests()
    {
        _validator = new GibraltarIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsGI()
    {
        _validator.CountryCode.Should().Be("GI");
    }

    [Theory]
    [InlineData("GI56XAPO000001234567890")]
    public void IsValidIban_WithValidGibraltarIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("GI56 XAPO 0000 0123 4567 890")]
    public void IsValidIban_WithFormattedIban_ReturnsTrue(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("GI00XAPO000001234567890")] // Wrong check digits
    [InlineData("FR56XAPO000001234567890")] // Wrong country
    [InlineData("GI56XAPO00000123456789")] // Too short
    [InlineData("GI56XAPO0000012345678901")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string? iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}



