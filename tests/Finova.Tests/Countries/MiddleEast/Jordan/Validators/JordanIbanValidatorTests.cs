using Finova.Countries.MiddleEast.Jordan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Jordan.Validators;

public class JordanIbanValidatorTests
{
    private readonly JordanIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsJO() => _validator.CountryCode.Should().Be("JO");

    [Theory]
    [InlineData("JO94CBJO0010000000000131000302")]
    public void IsValidIban_WithValidJordanIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("JO94 CBJO 0010 0000 0000 0131 0003 02")] // With spaces
    [InlineData("jo94cbjo0010000000000131000302")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("JO00CBJO0010000000000131000302")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("JO94CBJO00100000000001310003")] // Too short
    [InlineData("JO94CBJO001000000000013100030299")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("JO94CBJO0010000000000131000302")]
    public void ValidateJordanIban_WithValidIbans_ReturnsTrue(string iban)
        => JordanIbanValidator.ValidateJordanIban(iban).IsValid.Should().BeTrue();
}
