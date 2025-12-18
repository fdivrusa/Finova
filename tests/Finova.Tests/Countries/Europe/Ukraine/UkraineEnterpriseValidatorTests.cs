using Finova.Countries.Europe.Ukraine.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ukraine;

public class UkraineEnterpriseValidatorTests
{
    [Theory]
    // Valid EDRPOU: 32855961 (Example found online)
    // Calculated valid example: 12345678
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
