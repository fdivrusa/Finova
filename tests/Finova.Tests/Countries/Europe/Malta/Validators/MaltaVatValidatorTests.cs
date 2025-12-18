using Finova.Countries.Europe.Malta.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Malta.Validators;

public class MaltaVatValidatorTests
{
    [Theory]
    [InlineData("MT11679112", true)]
    [InlineData("11679112", true)]
    [InlineData("MT11679113", false)]
    [InlineData("11679113", false)]
    [InlineData("MT12345678", false)]
    [InlineData("12345678", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Validate_ReturnsExpectedResult(string? vat, bool expected)
    {
        var result = MaltaVatValidator.ValidateVat(vat);
        Assert.Equal(expected, result.IsValid);
    }
}
