using Finova.Countries.Africa.Kenya.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Africa.Kenya.Validators;

public class KenyaNationalIdValidatorTests
{
    [Theory]
    [InlineData("12345678")] // Valid length (8)
    [InlineData("1234567")] // Valid length (7)
    [InlineData("123456789")] // Valid length (9)
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = KenyaNationalIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123456")] // Too short
    [InlineData("1234567890")] // Too long
    [InlineData("1234567A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = KenyaNationalIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new KenyaNationalIdValidator().Parse(" 12345678 ");
        result.Should().Be("12345678");
    }
}
