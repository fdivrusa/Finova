using Finova.Countries.Africa.Egypt.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Africa.Egypt.Validators;

public class EgyptNationalIdValidatorTests
{
    [Theory]
    [InlineData("28001010100005")]
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = EgyptNationalIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("28001010100000")] // Invalid checksum
    [InlineData("48001010100005")] // Invalid century (4)
    [InlineData("28013010100005")] // Invalid month (13)
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = EgyptNationalIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new EgyptNationalIdValidator().Parse(" 28001010100005 ");
        result.Should().Be("28001010100005");
    }
}
