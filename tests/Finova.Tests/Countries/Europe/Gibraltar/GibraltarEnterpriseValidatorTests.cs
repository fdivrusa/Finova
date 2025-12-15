using Finova.Countries.Europe.Gibraltar.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Gibraltar;

public class GibraltarEnterpriseValidatorTests
{
    [Theory]
    [InlineData("12345", true)] // 5 digits
    [InlineData("123456", true)] // 6 digits
    [InlineData("1234567", true)] // 7 digits
    [InlineData("GI12345", true)] // Prefix
    [InlineData("1234", false)] // Too short
    [InlineData("12345678", false)] // Too long
    [InlineData("ABCDE", false)] // Non-numeric
    [InlineData(null, false)]
    [InlineData("", false)]
    public void CompanyNumber_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GibraltarCompanyNumberValidator.ValidateCompanyNumber(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
