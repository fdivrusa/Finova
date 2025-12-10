using Finova.Countries.Europe.Netherlands.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Netherlands.Validators;

public class NetherlandsIbanValidatorTests
{
    private readonly NetherlandsIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsNL() => _validator.CountryCode.Should().Be("NL");

    [Theory]
    [InlineData("NL91ABNA0417164300")]
    [InlineData("NL39RABO0300065264")]
    public void IsValidIban_WithValidDutchIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("NL91 ABNA 0417 1643 00")] // With spaces
    [InlineData("nl91abna0417164300")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("NL00ABNA0417164300")] // Wrong check digits
    [InlineData("BE68539007547034")] // Wrong country
    [InlineData("NL91ABNA04171643")] // Too short
    [InlineData("NL91AB1A0417164300")] // Bank code with digit
    [InlineData("NL91ABNA041716430X")] // Account with letter
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("NL91ABNA0417164300")]
    public void ValidateDutchIban_WithValidIbans_ReturnsTrue(string iban)
        => NetherlandsIbanValidator.ValidateNetherlandsIban(iban).IsValid.Should().BeTrue();
}


