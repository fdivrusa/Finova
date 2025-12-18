using Finova.Countries.Europe.Vatican.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Vatican;

public class VaticanCityEnterpriseValidatorTests
{
    [Theory]
    // Valid VAT (Italian format): 00462350018 (Example)
    // Luhn check on 00462350018.
    // 0 0 4 6 2 3 5 0 0 1 8
    // Weights (right to left, starting even position from right (1-indexed)): 1, 2, 1, 2...
    // Or standard Luhn: double every second digit from right.
    // 8 (1st), 1*2=2 (2nd), 0 (3rd), 0*2=0 (4th), 5 (5th), 3*2=6 (6th), 2 (7th), 6*2=12->1+2=3 (8th), 4 (9th), 0*2=0 (10th), 0 (11th).
    // Sum: 8+2+0+0+5+6+2+3+4+0+0 = 30.
    // 30 % 10 == 0. Valid.
    [InlineData("00462350018", true)]
    [InlineData("VA 00462350018", true)] // Valid with prefix
    [InlineData("IT 00462350018", true)] // Valid with IT prefix
    [InlineData("00462350019", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = VaticanCityVatValidator.ValidateVat(number);
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var result = VaticanCityVatValidator.Normalize("VA 00462350018");
        result.Should().Be("00462350018");
    }

    [Fact]
    public void Normalize_WithITPrefix_ShouldReturnCleanedNumber()
    {
        var result = VaticanCityVatValidator.Normalize("IT 00462350018");
        result.Should().Be("00462350018");
    }

    [Fact]
    public void Normalize_WithInvalidFormat_ShouldReturnNull()
    {
        var result = VaticanCityVatValidator.Normalize("INVALID");
        result.Should().BeNull();
    }
}
