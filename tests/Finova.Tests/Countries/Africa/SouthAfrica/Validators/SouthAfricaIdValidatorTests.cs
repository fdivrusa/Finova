using Finova.Countries.Africa.SouthAfrica.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Africa.SouthAfrica.Validators;

public class SouthAfricaIdValidatorTests
{
    [Theory]
    [InlineData("8001015000086")] // Valid ID
    [InlineData("800101 5000 086")] // Valid ID with spaces
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = SouthAfricaIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("9202204720080")] // Invalid checksum
    [InlineData("9213204720082")] // Invalid month (13)
    [InlineData("920220472008A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = SouthAfricaIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new SouthAfricaIdValidator().Parse(" 800101 5000 086 ");
        result.Should().Be("8001015000086");
    }
}
