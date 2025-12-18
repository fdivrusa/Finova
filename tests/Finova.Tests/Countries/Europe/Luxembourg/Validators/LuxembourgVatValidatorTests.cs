using Finova.Countries.Europe.Luxembourg.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Luxembourg.Validators;

public class LuxembourgVatValidatorTests
{
    [Theory]
    [InlineData("LU15027442", true)]
    [InlineData("15027442", true)]
    [InlineData("LU15027443", false)]
    [InlineData("15027443", false)]
    [InlineData("LU12345678", false)]
    [InlineData("12345678", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Validate_ReturnsExpectedResult(string? vat, bool expected)
    {
        var result = LuxembourgVatValidator.ValidateVat(vat);
        Assert.Equal(expected, result.IsValid);
    }
}
