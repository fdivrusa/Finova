using Finova.Countries.SouthAmerica.Colombia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Colombia.Validators;

public class ColombiaCedulaValidatorTests
{
    [Theory]
    [InlineData("1234567890")] // Valid length (10)
    [InlineData("123456")] // Valid length (6)
    [InlineData("1.234.567.890")] // With dots
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = ColombiaCedulaValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("12345")] // Too short
    [InlineData("12345678901")] // Too long
    [InlineData("123456789A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = ColombiaCedulaValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new ColombiaCedulaValidator().Parse(" 1.234.567.890 ");
        result.Should().Be("1234567890");
    }
}
