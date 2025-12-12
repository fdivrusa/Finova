using Finova.Countries.Europe.Italy.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Italy.Validators;

public class ItalyVatValidatorTests
{
    [Theory]
    [InlineData("IT00000010215")] // Valid
    [InlineData("00000010215")] // Valid without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = ItalyVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IT00000010216")] // Invalid Luhn
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = ItalyVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
