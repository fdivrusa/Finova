using Finova.Countries.NorthAmerica.Guatemala.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.Guatemala.Validators;

public class GuatemalaIbanValidatorTests
{
    private readonly GuatemalaIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsGT() => _validator.CountryCode.Should().Be("GT");

    [Theory]
    [InlineData("GT82TRAJ01020000001210029690")]
    public void IsValidIban_WithValidGuatemalaIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("GT82 TRAJ 0102 0000 0012 1002 9690")] // With spaces
    [InlineData("gt82traj01020000001210029690")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("GT00TRAJ01020000001210029690")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("GT82TRAJ0102000000121002")] // Too short
    [InlineData("GT82TRAJ0102000000121002969099")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("GT82TRAJ01020000001210029690")]
    public void ValidateGuatemalaIban_WithValidIbans_ReturnsTrue(string iban)
        => GuatemalaIbanValidator.ValidateGuatemalaIban(iban).IsValid.Should().BeTrue();
}
