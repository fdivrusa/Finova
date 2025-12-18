using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.UnitedKingdom;

public class UnitedKingdomNinoValidatorTests
{
    private readonly UnitedKingdomNinoValidator _validator = new();

    [Theory]
    [InlineData("PB123456C")]
    [InlineData("AA123456A")]
    [InlineData("AB123456D")]
    [InlineData("  PB 123456 C  ")] // With spaces
    public void Validate_ValidNino_ReturnsSuccess(string nino)
    {
        var result = _validator.Validate(nino);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? nino)
    {
        var result = _validator.Validate(nino);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("QQ123456")] // Too short
    [InlineData("QQ12345678")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string nino)
    {
        var result = _validator.Validate(nino);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("GB123456A")] // Invalid prefix GB
    [InlineData("BG123456A")] // Invalid prefix BG
    [InlineData("NK123456A")] // Invalid prefix NK
    [InlineData("TN123456A")] // Invalid prefix TN
    [InlineData("ZZ123456A")] // Invalid prefix ZZ
    [InlineData("D1123456A")] // Invalid first char D
    [InlineData("A0123456A")] // Invalid second char 0
    [InlineData("AA123456E")] // Invalid suffix E
    public void Validate_InvalidFormat_ReturnsFailure(string nino)
    {
        var result = _validator.Validate(nino);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidFormat);
    }
}
