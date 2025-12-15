using Finova.Countries.Europe.SanMarino.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.SanMarino;

public class SanMarinoEnterpriseValidatorTests
{
    [Theory]
    [InlineData("12345", true)] // Valid COE
    [InlineData("SM 12345", false)] // Prefix not stripped by validator? Validator strips spaces but not SM prefix?
    // SanMarinoCoeValidator strips spaces. Does it strip SM?
    // My implementation: var cleaned = number.Replace(" ", "");
    // It does NOT strip SM.
    // Wait, usually I strip country code.
    // Let's check implementation.
    // public static ValidationResult ValidateCoe(string? number) { ... var cleaned = number.Replace(" ", ""); ... }
    // I should probably strip SM if it's common.
    // But for now, let's test without SM or update implementation.
    // The prompt said "Sanitization: Remove spaces." It didn't say Remove 'SM'.
    // But usually we do.
    // I'll assume strict 5 digits.
    [InlineData("1234", false)] // Invalid length
    [InlineData("123456", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = SanMarinoCoeValidator.ValidateCoe(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var validator = new SanMarinoCoeValidator();
        var result = validator.Normalize("12345");
        Assert.Equal("12345", result);
    }
}
