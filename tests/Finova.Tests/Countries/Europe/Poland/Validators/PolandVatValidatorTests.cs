using Finova.Countries.Europe.Poland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Poland.Validators;

public class PolandVatValidatorTests
{
    [Theory]
    [InlineData("PL5260300291")] // Valid (Oracle Polska)
    [InlineData("5260300291")] // Valid without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = PolandVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL5260300292")] // Invalid checksum
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        var result = PolandVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }
}
