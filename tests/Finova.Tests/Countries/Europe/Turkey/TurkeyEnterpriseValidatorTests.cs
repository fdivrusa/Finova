using Finova.Countries.Europe.Turkey.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Turkey;

public class TurkeyEnterpriseValidatorTests
{
    [Theory]
    // Valid VKN: 0010054536 (Calculated example)
    [InlineData("0010054536", true)]
    [InlineData("TR 0010054536", true)] // Valid with prefix
    [InlineData("0010054537", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = TurkeyVknValidator.ValidateVkn(number);
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var result = TurkeyVknValidator.Normalize("TR 0010054536");
        result.Should().Be("0010054536");
    }

    [Fact]
    public void Normalize_WithInvalidFormat_ShouldReturnNull()
    {
        var result = TurkeyVknValidator.Normalize("INVALID");
        result.Should().BeNull();
    }
}
