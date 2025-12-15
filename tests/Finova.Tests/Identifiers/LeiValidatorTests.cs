using Finova.Core.Common;
using Finova.Core.Identifiers;
using Xunit;

namespace Finova.Tests.Identifiers;

public class LeiValidatorTests
{
    private readonly LeiValidator _validator = new();

    [Theory]
    [InlineData("2138008ANE5QKSVFL345")] // Irwing Mitchell
    [InlineData("8I5DZWZKVSZI1NUHU748")] // JP morgan
    public void Validate_ValidLei_ReturnsSuccess(string lei)
    {
        // Act
        var result = _validator.Validate(lei);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyInput_ReturnsInvalidInput(string? input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("5493001KJTIIGC8Y1R1")] // Too short (19)
    [InlineData("5493001KJTIIGC8Y1R177")] // Too long (21)
    public void Validate_InvalidLength_ReturnsInvalidLength(string lei)
    {
        // Act
        var result = _validator.Validate(lei);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidLength, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("5493001KJTIIGC8Y1R18")] // Checksum mismatch (last digit changed)
    [InlineData("5493001KJTIIGC8Y1R16")] // Checksum mismatch
    public void Validate_InvalidChecksum_ReturnsInvalidChecksum(string lei)
    {
        // Act
        var result = _validator.Validate(lei);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidChecksum, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("5493001KJTIIGC8Y1R1!")] // Special char
    [InlineData("5493001KJTIIGC8Y1R1-")] // Hyphen
    public void Validate_InvalidFormat_ReturnsInvalidFormat(string lei)
    {
        // Act
        var result = _validator.Validate(lei);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.Errors[0].Code);
    }

    [Fact]
    public void Parse_ValidInput_ReturnsNormalized()
    {
        // Arrange
        string input = "  5493001KJTIIGC8Y1R17  ";

        // Act
        string? result = _validator.Parse(input);

        // Assert
        Assert.Equal("5493001KJTIIGC8Y1R17", result);
    }

    [Fact]
    public void Parse_NullInput_ReturnsNull()
    {
        // Act
        string? result = _validator.Parse(null);

        // Assert
        Assert.Null(result);
    }
}
