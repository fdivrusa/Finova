using Finova.Countries.Asia.Pakistan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Pakistan.Validators;

public class PakistanIbanValidatorTests
{
    private readonly PakistanIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsPK() => _validator.CountryCode.Should().Be("PK");

    [Theory]
    [InlineData("PK36SCBL0000001123456702")]
    public void IsValidIban_WithValidPakistanIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("PK36 SCBL 0000 0011 2345 6702")] // With spaces
    [InlineData("pk36scbl0000001123456702")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("PK00SCBL0000001123456702")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("PK36SCBL000000112345")] // Too short
    [InlineData("PK36SCBL000000112345670299")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("PK36SCBL0000001123456702")]
    public void ValidatePakistanIban_WithValidIbans_ReturnsTrue(string iban)
        => PakistanIbanValidator.ValidatePakistanIban(iban).IsValid.Should().BeTrue();
}
