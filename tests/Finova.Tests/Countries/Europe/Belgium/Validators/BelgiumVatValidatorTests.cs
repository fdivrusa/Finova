using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Validators;

public class BelgiumVatValidatorTests
{
    [Theory]
    [InlineData("0403019261")] // Valid 10 digit
    [InlineData("403019261")]  // Valid 9 digit (should auto-correct)
    [InlineData("BE0403019261")] // Valid with prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = BelgiumVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("0403019262")] // Invalid checksum
    [InlineData("123")] // Too short
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = BelgiumVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
