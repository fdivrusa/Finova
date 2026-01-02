using Finova.Countries.SoutheastAsia.Vietnam.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SoutheastAsia.Vietnam.Validators;

public class VietnamCitizenIdValidatorTests
{
    [Theory]
    [InlineData("001080000001")] // Valid length (12)
    [InlineData(" 001080000001 ")] // With spaces
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = VietnamCitizenIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("00108000001")] // Too short
    [InlineData("0010800000001")] // Too long
    [InlineData("00108000000A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = VietnamCitizenIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new VietnamCitizenIdValidator().Parse(" 001080000001 ");
        result.Should().Be("001080000001");
    }
}
