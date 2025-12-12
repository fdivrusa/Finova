using Finova.Countries.Europe.Germany.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany.Validators;

public class GermanyVatValidatorTests
{
    [Theory]
    [InlineData("DE123456788")] // Valid (Calculated: 12345678 -> Check 8)
    [InlineData("123456788")] // Valid without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = GermanyVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("DE123456789")] // Invalid checksum
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = GermanyVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
