using Finova.Countries.Europe.Cyprus.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Cyprus.Validators;

public class CyprusVatValidatorTests
{
    [Theory]
    [InlineData("10259033P")] // Valid
    [InlineData("CY10259033P")] // Valid with prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = CyprusVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("10259033Z")] // Invalid checksum
    [InlineData("12345678X")] // Invalid format
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = CyprusVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
