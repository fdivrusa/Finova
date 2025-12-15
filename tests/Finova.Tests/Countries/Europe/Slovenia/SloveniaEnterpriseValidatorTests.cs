using Finova.Countries.Europe.Slovenia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovenia;

public class SloveniaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("15012557", true)] // Valid VAT
    [InlineData("SI 15012557", true)] // Valid with prefix
    [InlineData("15012558", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = SloveniaVatValidator.ValidateVat(number);
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var result = SloveniaVatValidator.Normalize("SI 15012557");
        result.Should().Be("15012557");
    }

    [Fact]
    public void Normalize_WithInvalidFormat_ShouldReturnNull()
    {
        var result = SloveniaVatValidator.Normalize("INVALID");
        result.Should().BeNull();
    }
}
