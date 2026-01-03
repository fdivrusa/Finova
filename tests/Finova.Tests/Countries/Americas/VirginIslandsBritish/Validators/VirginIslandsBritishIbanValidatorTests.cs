using Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.VirginIslandsBritish.Validators;

public class VirginIslandsBritishIbanValidatorTests
{
    private readonly VirginIslandsBritishIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsVG() => _validator.CountryCode.Should().Be("VG");

    [Theory]
    [InlineData("VG96VPVG0000012345678901")]
    public void IsValidIban_WithValidVirginIslandsIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("VG96 VPVG 0000 0123 4567 8901")] // With spaces
    [InlineData("vg96vpvg0000012345678901")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("VG00VPVG0000012345678901")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("VG96VPVG00000123456")] // Too short
    [InlineData("VG96VPVG000001234567890199")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("VG96VPVG0000012345678901")]
    public void ValidateVirginIslandsBritishIban_WithValidIbans_ReturnsTrue(string iban)
        => VirginIslandsBritishIbanValidator.ValidateVirginIslandsBritishIban(iban).IsValid.Should().BeTrue();
}
