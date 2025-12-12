using Finova.Countries.Europe.Andorra.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Andorra.Validators;

public class AndorraVatValidatorTests
{
    [Theory]
    [InlineData("U123456A")]
    [InlineData("F001234A")]
    [InlineData("A999999Z")]
    public void IsValid_WithValidVatNumbers_ReturnsTrue(string vat)
    {
        // Act
        var result = AndorraVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("U123456A", "ADU123456A")]
    [InlineData("ADU123456A", "ADU123456A")]
    public void Parse_ReturnsCorrectDetails(string input, string expectedVatNumber)
    {
        // Act
        var result = new AndorraVatValidator().Parse(input);

        // Assert
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be("AD");
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData("1234567A")] // Missing first letter
    [InlineData("U12345A")] // Too short
    [InlineData("U1234567A")] // Too long
    [InlineData("12345678")] // All digits
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_WithInvalidVatNumbers_ReturnsFalse(string? vat)
    {
        // Act
        var result = AndorraVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}

