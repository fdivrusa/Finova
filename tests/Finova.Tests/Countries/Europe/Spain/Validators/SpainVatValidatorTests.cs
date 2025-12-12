using Finova.Countries.Europe.Spain.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Spain.Validators;

public class SpainVatValidatorTests
{
    [Theory]
    [InlineData("ESB86480944")] // Valid CIF (Calculated)
    [InlineData("B86480944")] // Valid CIF without prefix
    [InlineData("12345678Z")] // Valid DNI (Calculated)
    [InlineData("X1234567L")] // Valid NIE (Calculated)
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = SpainVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ESB86480946")] // Invalid CIF
    [InlineData("12345678A")] // Invalid DNI
    [InlineData("X1234567A")] // Invalid NIE
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = SpainVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
