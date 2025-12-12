using Finova.Countries.Europe.France.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.France.Validators;

public class FranceVatValidatorTests
{
    [Theory]
    [InlineData("FR44732829320")] // Valid (Key 44, SIREN 732829320)
    [InlineData("44732829320")] // Valid without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = FranceVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FR01300025705")] // Invalid Key
    [InlineData("FR00300025706")] // Invalid SIREN Luhn
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = FranceVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
