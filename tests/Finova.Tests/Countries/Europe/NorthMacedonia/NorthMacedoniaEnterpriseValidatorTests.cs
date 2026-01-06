using Finova.Countries.Europe.NorthMacedonia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.NorthMacedonia;

public class NorthMacedoniaEnterpriseValidatorTests
{
    [Theory]
    // Valid EDB calculated: 4030992250004
    // Let's calculate one.
    // Weights: 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2
    // Data: 403099225000
    // Sum: 4*7 + 0*6 + 3*5 + 0*4 + 9*3 + 9*2 + 2*7 + 2*6 + 5*5 + 0*4 + 0*3 + 0*2
    // = 28 + 0 + 15 + 0 + 27 + 18 + 14 + 12 + 25 + 0 + 0 + 0
    // = 139
    // Remainder = 139 % 11 = 7
    // CheckDigit = 11 - 7 = 4.
    // So 4030992250004 should be valid.
    [InlineData("4030992250004", true)]
    [InlineData("4030992250000", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    [InlineData("", false)] // Empty
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = NorthMacedoniaEdbValidator.ValidateEdb(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var validator = new NorthMacedoniaEdbValidator();
        var result = validator.Normalize("4030992 250004");
        Assert.Equal("4030992250004", result);
    }
}
