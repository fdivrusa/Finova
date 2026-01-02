using Finova.Countries.SoutheastAsia.Indonesia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SoutheastAsia.Indonesia.Validators;

public class IndonesiaNikValidatorTests
{
    [Theory]
    [InlineData("3201010101800001")] // Valid: 1980-01-01, Male
    [InlineData("3201014101800001")] // Valid: 1980-01-01, Female (01+40=41)
    [InlineData("32.01.01.010180.0001")] // With dots
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = IndonesiaNikValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("3201010113800001")] // Invalid month (13)
    [InlineData("320101010180000A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = IndonesiaNikValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new IndonesiaNikValidator().Parse(" 32.01.01.010180.0001 ");
        result.Should().Be("3201010101800001");
    }
}
