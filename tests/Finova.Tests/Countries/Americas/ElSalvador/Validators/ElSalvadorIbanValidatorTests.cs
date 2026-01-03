using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.ElSalvador.Validators;

public class ElSalvadorIbanValidatorTests
{
    private readonly ElSalvadorIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsSV() => _validator.CountryCode.Should().Be("SV");

    [Theory]
    [InlineData("SV62CENR00000000000000700025")]
    public void IsValidIban_WithValidElSalvadorIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("SV62 CENR 0000 0000 0000 0070 0025")] // With spaces
    [InlineData("sv62cenr00000000000000700025")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("SV00CENR00000000000000700025")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("SV62CENR000000000000007")] // Too short
    [InlineData("SV62CENR0000000000000070002599")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("SV62CENR00000000000000700025")]
    public void ValidateElSalvadorIban_WithValidIbans_ReturnsTrue(string iban)
        => ElSalvadorIbanValidator.ValidateElSalvadorIban(iban).IsValid.Should().BeTrue();
}
