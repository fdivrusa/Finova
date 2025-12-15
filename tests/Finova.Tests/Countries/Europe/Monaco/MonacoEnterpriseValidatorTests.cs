using Finova.Countries.Europe.Monaco.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Monaco;

public class MonacoEnterpriseValidatorTests
{
    [Theory]
    [InlineData("23S12345", true)]
    [InlineData("23P12345", true)]
    [InlineData("23 S 12345", true)] // Spaces allowed and stripped
    [InlineData("23A12345", false)] // Invalid type
    [InlineData("123S12345", false)] // Invalid year length
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Rci_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = MonacoRciValidator.ValidateRci(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
