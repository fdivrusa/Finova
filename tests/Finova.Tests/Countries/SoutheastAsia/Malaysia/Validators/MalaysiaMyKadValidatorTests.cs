using Finova.Countries.SoutheastAsia.Malaysia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SoutheastAsia.Malaysia.Validators;

public class MalaysiaMyKadValidatorTests
{
    [Theory]
    [InlineData("800101-01-1234")] // Valid: 1980-01-01
    [InlineData("800101011234")] // Without hyphens
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = MalaysiaMyKadValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("801301011234")] // Invalid month (13)
    [InlineData("80010101123A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = MalaysiaMyKadValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new MalaysiaMyKadValidator().Parse(" 800101-01-1234 ");
        result.Should().Be("800101011234");
    }
}
