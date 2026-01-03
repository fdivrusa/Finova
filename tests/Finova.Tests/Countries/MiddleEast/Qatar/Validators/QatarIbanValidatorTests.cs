using Finova.Countries.MiddleEast.Qatar.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Qatar.Validators;

public class QatarIbanValidatorTests
{
    private readonly QatarIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsQA() => _validator.CountryCode.Should().Be("QA");

    [Theory]
    [InlineData("QA58DOHB00001234567890ABCDEFG")]
    public void IsValidIban_WithValidQatarIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("QA58 DOHB 0000 1234 5678 90AB CDEF G")] // With spaces
    [InlineData("qa58dohb00001234567890abcdefg")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("QA00DOHB00001234567890ABCDEFG")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("QA58DOHB00001234567890ABC")] // Too short
    [InlineData("QA58DOHB00001234567890ABCDEFGH99")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("QA58DOHB00001234567890ABCDEFG")]
    public void ValidateQatarIban_WithValidIbans_ReturnsTrue(string iban)
        => QatarIbanValidator.ValidateQatarIban(iban).IsValid.Should().BeTrue();
}
