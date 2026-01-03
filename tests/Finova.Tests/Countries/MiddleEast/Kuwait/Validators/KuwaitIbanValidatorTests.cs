using Finova.Countries.MiddleEast.Kuwait.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Kuwait.Validators;

public class KuwaitIbanValidatorTests
{
    private readonly KuwaitIbanValidator _validator = new();

    [Fact]
    public void CountryCode_ReturnsKW() => _validator.CountryCode.Should().Be("KW");

    [Theory]
    [InlineData("KW81CBKU0000000000001234560101")]
    public void IsValidIban_WithValidKuwaitIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("KW81 CBKU 0000 0000 0000 1234 5601 01")] // With spaces
    [InlineData("kw81cbku0000000000001234560101")] // Lowercase
    public void IsValidIban_WithFormattedIbans_ReturnsTrue(string iban)
        => _validator.Validate(iban).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("KW00CBKU0000000000001234560101")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("KW81CBKU000000000000123456")] // Too short
    [InlineData("KW81CBKU000000000000123456010199")] // Too long
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void IsValidIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => _validator.Validate(iban).IsValid.Should().BeFalse();

    [Theory]
    [InlineData("KW81CBKU0000000000001234560101")]
    public void ValidateKuwaitIban_WithValidIbans_ReturnsTrue(string iban)
        => KuwaitIbanValidator.ValidateKuwaitIban(iban).IsValid.Should().BeTrue();
}
