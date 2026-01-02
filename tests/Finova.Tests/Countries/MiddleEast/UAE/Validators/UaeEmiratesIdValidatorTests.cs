using Finova.Countries.MiddleEast.UAE.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.UAE.Validators;

public class UaeEmiratesIdValidatorTests
{
    [Theory]
    [InlineData("784-1980-1234567-8")] // Valid format and checksum
    [InlineData("784198412345674")]
    [InlineData("784-1984-1234567-4")]
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = UaeEmiratesIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("784198412345670")] // Invalid checksum
    [InlineData("123198412345672")] // Invalid start (not 784)
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = UaeEmiratesIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new UaeEmiratesIdValidator().Parse(" 784-1984-1234567-4 ");
        result.Should().Be("784198412345674");
    }
}
