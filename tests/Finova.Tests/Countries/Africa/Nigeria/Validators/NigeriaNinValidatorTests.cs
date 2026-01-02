using Finova.Countries.Africa.Nigeria.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Africa.Nigeria.Validators;

public class NigeriaNinValidatorTests
{
    [Theory]
    [InlineData("12345678901")] // Valid length (11)
    [InlineData(" 12345678901 ")] // With spaces
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = NigeriaNinValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1234567890")] // Too short
    [InlineData("123456789012")] // Too long
    [InlineData("1234567890A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = NigeriaNinValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new NigeriaNinValidator().Parse(" 12345678901 ");
        result.Should().Be("12345678901");
    }
}
