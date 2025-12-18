using Finova.Countries.Europe.Italy.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Italy.Validators;

public class ItalyNationalIdValidatorTests
{
    [Theory]
    [InlineData("RSSMRA80A01H501U")] // Valid
    [InlineData("VRDGLC70M20L736A")] // Valid
    [InlineData("RSS MRA 80A01 H501 U")] // Valid with spaces
    public void Validate_ValidId_ReturnsSuccess(string input)
    {
        // Act
        var result = ItalyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        // Act
        var result = ItalyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("RSSMRA80A01H501")] // Too short
    [InlineData("RSSMRA80A01H501UU")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        // Act
        var result = ItalyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("RSSMRA80A01H5011")] // Last char must be letter
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        // Act
        var result = ItalyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("RSSMRA80A01H501A")] // Invalid checksum (Should be U)
    [InlineData("VRDGLC70M20L736B")] // Invalid checksum (Should be A)
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        // Act
        var result = ItalyNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidChecksum);
    }
}
