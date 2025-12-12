using Finova.Countries.Europe.Ukraine.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ukraine.Validators;

public class UkraineIbanValidatorTests
{
    private readonly UkraineIbanValidator _validator;

    public UkraineIbanValidatorTests()
    {
        _validator = new UkraineIbanValidator();
    }

    [Fact]
    public void CountryCode_ReturnsUA()
    {
        _validator.CountryCode.Should().Be("UA");
    }

    [Fact]
    public void IsValidIban_WithValidUkraineIban_ReturnsTrue()
    {
        // Valid IBAN calculated
        var iban = "UA102139962200000260072335660";
        _validator.Validate(iban).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidIban_WithFormattedIban_ReturnsTrue()
    {
        var iban = "UA10 2139 9622 0000 0260 0723 3566 0";
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
    [InlineData("UA10213996220000026007233566")] // Too short
    [InlineData("UA1021399622000002600723356600")] // Too long
    [InlineData("XX102139962200000260072335660")] // Wrong country code
    [InlineData("UA102139962200000260072335661")] // Invalid checksum
    public void IsValidIban_WithInvalidIban_ReturnsFalse(string iban)
    {
        _validator.Validate(iban).IsValid.Should().BeFalse();
    }
}

