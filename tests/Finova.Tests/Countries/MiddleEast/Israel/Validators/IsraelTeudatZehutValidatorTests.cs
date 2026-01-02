using Finova.Countries.MiddleEast.Israel.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Israel.Validators;

public class IsraelTeudatZehutValidatorTests
{
    [Theory]
    [InlineData("300000007")] // Valid ID
    [InlineData("123456782")] // Valid ID
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = IsraelTeudatZehutValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123456780")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ABC")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = IsraelTeudatZehutValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new IsraelTeudatZehutValidator().Parse(" 123456782 ");
        result.Should().Be("123456782");
    }
}
