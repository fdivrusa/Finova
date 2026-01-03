using Finova.Countries.Africa.Egypt.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Africa.Egypt.Validators;

public class EgyptIbanValidatorTests
{
    private readonly EgyptIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsEG() => _validator.CountryCode.Should().Be("EG");

    [Theory]
    [InlineData("EG380019000500000000263180002")]
    public void IsValidIban_WithValidEgyptIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("EG38 0019 0005 0000 0000 2631 8000 2")] // With spaces
    [InlineData("eg380019000500000000263180002")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("EG000019000500000000263180002")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("EG3800190005000000002631800")] // Too short
    [InlineData("EG38001900050000000026318000299")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("EG380019000500000000263180002")]
    public void ValidateEgyptIban_WithValidIbans_ReturnsTrue(string iban)
        => EgyptIbanValidator.ValidateEgyptIban(iban).IsValid.Should().BeTrue();
}
