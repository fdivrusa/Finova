using Finova.Countries.Europe.Albania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Albania.Validators;

public class AlbaniaVatValidatorTests
{
    [Theory]
    [InlineData("K31415037M")]
    [InlineData("K87654321A")]
    [InlineData("L11223344Z")]
    public void IsValid_WithValidVatNumbers_ReturnsTrue(string vat)
    {
        // Act
        var result = AlbaniaVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("K31415037M", "ALK31415037M")]
    [InlineData("ALK31415037M", "ALK31415037M")]
    public void Parse_ReturnsCorrectDetails(string input, string expectedVatNumber)
    {
        // Act
        var result = new AlbaniaVatValidator().Parse(input);

        // Assert
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be("AL");
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData("J1234567L")] // Too short
    [InlineData("J123456789L")] // Too long
    [InlineData("J123456789")] // Missing last letter
    [InlineData("1234567890")] // All digits
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_WithInvalidVatNumbers_ReturnsFalse(string? vat)
    {
        // Act
        var result = AlbaniaVatValidator.Validate(vat);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}

