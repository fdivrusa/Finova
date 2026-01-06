using Finova.Countries.SouthAmerica.Colombia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Colombia;

public class ColombiaVatValidatorTests
{
    [Theory]
    [InlineData("123456789")] // 9-digit NIT without check digit
    [InlineData("CO123456789")] // With CO prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = ColombiaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345678")] // Too short (8 digits)
    [InlineData("12345678901")] // Too long (11 digits)
    [InlineData("ABCDEFGHIJ")] // Non-numeric
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = ColombiaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_With9Digits_ReturnsSuccess()
    {
        var result = ColombiaVatValidator.Validate("123456789");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "123456789";
        var result = ColombiaVatValidator.GetVatDetails(vat);

        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("CO");
        result.IdentifierKind.Should().Be("NIT");
        result.IsEuVat.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("12345678")] // Too short
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = ColombiaVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
