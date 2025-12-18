using Finova.Countries.Europe.Luxembourg.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Luxembourg;

public class LuxembourgNationalIdValidatorTests
{
    private readonly LuxembourgNationalIdValidator _validator = new();

    [Theory]
    [InlineData("1980122512346")] // Valid Matricule
    // 19801225123 / 97 = 204136341.474...
    // 204136341 * 97 = 19801225077
    // 19801225123 - 19801225077 = 46. Wait.
    // Let's recalculate.
    // 19801225123 % 97.
    // 19801225123 = 204136341 * 97 + 46.
    // So remainder is 46.
    // So valid ID should end in 46.
    public void Validate_ValidMatricule_ReturnsSuccess(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Input cannot be empty.");
    }

    [Theory]
    [InlineData("1980122512")] // Too short
    [InlineData("19801225123456")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("1980132512346")] // Invalid Month 13
    public void Validate_InvalidDate_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid format.");
    }

    [Theory]
    [InlineData("1980122512399")] // Invalid checksum (should be 46)
    public void Validate_InvalidChecksum_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid checksum.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" 19801225-123-46 ");
        result.Should().Be("1980122512346");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
