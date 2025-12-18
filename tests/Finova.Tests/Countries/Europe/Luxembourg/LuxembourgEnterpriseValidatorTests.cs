using Finova.Countries.Europe.Luxembourg.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Luxembourg;

public class LuxembourgEnterpriseValidatorTests
{
    [Theory]
    [InlineData("LU10000356", true)]
    public void Tva_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = LuxembourgVatValidator.ValidateVat(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }


}
