using Finova.Countries.Europe.Netherlands.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Netherlands.Validators;

public class NetherlandsVatValidatorTests
{
    [Theory]
    [InlineData("NL858327694B63")] // Valid (Calculated Mod 97)
    [InlineData("858327694B63")] // Valid without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = NetherlandsVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NL858327694B02")] // Invalid checksum
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = NetherlandsVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
