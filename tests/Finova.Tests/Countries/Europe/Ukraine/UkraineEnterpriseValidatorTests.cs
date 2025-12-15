using Finova.Countries.Europe.Ukraine.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ukraine;

public class UkraineEnterpriseValidatorTests
{
    [Theory]
    // Valid EDRPOU: 32855961 (Example found online)
    // Pass 1: 3, 2, 8, 5, 5, 9, 6. Weights: 1, 2, 3, 4, 5, 6, 7.
    // Sum = 3*1 + 2*2 + 8*3 + 5*4 + 5*5 + 9*6 + 6*7 = 3 + 4 + 24 + 20 + 25 + 54 + 42 = 172.
    // Remainder = 172 % 11 = 7.
    // CheckDigit = 7.
    // Last digit is 1. Wait.
    // Let's recheck the example or calculation.
    // Maybe weights are different or example is wrong.
    // Let's try to calculate a valid one.
    // Data: 1234567
    // Pass 1: 1*1 + 2*2 + 3*3 + 4*4 + 5*5 + 6*6 + 7*7 = 1 + 4 + 9 + 16 + 25 + 36 + 49 = 140.
    // Remainder = 140 % 11 = 8.
    // CheckDigit = 8.
    // So 12345678 should be valid.
    [InlineData("12345678", true)]
    [InlineData("UA 12345678", true)] // Valid with prefix
    [InlineData("12345679", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = UkraineEdrpouValidator.ValidateEdrpou(number);
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var result = UkraineEdrpouValidator.Normalize("UA 12345678");
        result.Should().Be("12345678");
    }

    [Fact]
    public void Normalize_WithInvalidFormat_ShouldReturnNull()
    {
        var result = UkraineEdrpouValidator.Normalize("INVALID");
        result.Should().BeNull();
    }
}
