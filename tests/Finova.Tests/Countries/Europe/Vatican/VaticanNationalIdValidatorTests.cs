using Finova.Countries.Europe.Vatican.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Vatican;

public class VaticanNationalIdValidatorTests
{
    private readonly VaticanNationalIdValidator _validator = new();

    [Fact]
    public void Validate_ReturnsUnsupported()
    {
        var result = _validator.Validate("12345");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Vatican City National ID validation is not supported.");
    }

    [Fact]
    public void Parse_ReturnsNull()
    {
        var result = _validator.Parse("12345");
        result.Should().BeNull();
    }
}
