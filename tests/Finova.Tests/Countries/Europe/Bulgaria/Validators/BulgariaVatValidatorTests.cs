using Finova.Countries.Europe.Bulgaria.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Bulgaria.Validators;

public class BulgariaVatValidatorTests
{
    [Theory]
    [InlineData("100000001")] // Valid 9 digit
    [InlineData("BG100000001")] // Valid 9 digit with prefix
    [InlineData("0041010002")] // Valid 10 digit (EGN)
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = BulgariaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("100000005")] // Invalid checksum 9 digit
    [InlineData("0041010003")] // Invalid checksum 10 digit
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = BulgariaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
