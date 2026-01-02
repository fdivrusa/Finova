using Finova.Countries.SouthAmerica.Chile.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Chile.Validators;

public class ChileRutValidatorTests
{
    private readonly ChileRutValidator _validator = new();

    [Theory]
    [InlineData("12.345.678-5")] // Calculated valid
    [InlineData("12345678-5")] // Without dots
    [InlineData("123456785")] // Without hyphen
    [InlineData("30.686.957-4")] // Real example found online
    public void Validate_WithValidRut_ReturnsSuccess(string rut)
    {
        var result = ChileRutValidator.ValidateStatic(rut);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("12.345.678-0")] // Invalid checksum
    [InlineData("12.345.678-A")] // Invalid format (check digit can be K but not A)
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidRut_ReturnsFailure(string? rut)
    {
        var result = ChileRutValidator.ValidateStatic(rut);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidRut_ReturnsCleanRut()
    {
        var result = _validator.Parse("12.345.678-5");

        // Assert
        result.Should().Be("123456785");
    }
}
