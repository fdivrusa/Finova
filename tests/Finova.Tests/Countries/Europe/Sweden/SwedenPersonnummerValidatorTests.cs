using Finova.Countries.Europe.Sweden.Validators;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Sweden;

public class SwedenPersonnummerValidatorTests
{
    private readonly SwedenPersonnummerValidator _validator = new();

    [Theory]
    [InlineData("811218-9876")]
    [InlineData("19811218-9876")]
    [InlineData("811218+9876")] // Plus sign
    [InlineData("8112189876")] // No separator
    public void Validate_ValidPersonnummer_ReturnsSuccess(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("811218-987")] // Too short
    [InlineData("811218-98766")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("811218-9877")] // Invalid Luhn (last digit changed)
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Theory]
    [InlineData("811318-9876")] // Invalid month 13
    public void Validate_InvalidDate_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        // Could be InvalidFormat or InvalidChecksum depending on implementation details, 
        // but here we check date explicitly so likely InvalidFormat or custom message.
        // The validator returns InvalidFormat for invalid date.
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidFormat);
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var input = "811218-9876";
        var result = _validator.Parse(input);
        result.Should().Be("8112189876");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var input = "811218-9877"; // Invalid checksum
        var result = _validator.Parse(input);
        result.Should().BeNull();
    }
}
