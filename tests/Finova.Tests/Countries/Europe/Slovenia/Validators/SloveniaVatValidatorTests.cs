using Finova.Countries.Europe.Slovenia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovenia.Validators;

public class SloveniaVatValidatorTests
{
    [Theory]
    [InlineData("SI50223054")] // Valid Slovenian VAT (8 digits)
    [InlineData("50223054")] // Without prefix
    [InlineData("SI12345679")] // Valid format
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = SloveniaVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SI 5022 3054")] // With spaces
    [InlineData("si50223054")] // Lowercase
    [InlineData(" SI50223054 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = SloveniaVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("SI50223050")] // Wrong check digit
    [InlineData("SI5022305")] // Too short
    [InlineData("SI502230541")] // Too long
    [InlineData("SI5022305X")] // Contains letter in number
    [InlineData("XX50223054")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        // Act
        var result = SloveniaVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("SI50223054", "SI", "50223054")]
    [InlineData("50223054", "SI", "50223054")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        // Act
        var result = new SloveniaVatValidator().Parse(vat);

        // Assert
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("SI50223050")] // Invalid check digit
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        // Act
        var result = new SloveniaVatValidator().Parse(vat);

        // Assert
        result.Should().BeNull();
    }
}
