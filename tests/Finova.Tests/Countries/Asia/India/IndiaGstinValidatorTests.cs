using Finova.Countries.Asia.India.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.India;

public class IndiaGstinValidatorTests
{
    // Note: GSTIN has a modified Luhn checksum algorithm.
    // Tests focus on format validation and checksum rejection.

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("22AAAAA0000A1Z")] // Too short (14 chars)
    [InlineData("22AAAAA0000A1Z56")] // Too long (16 chars)
    [InlineData("00AAAAA0000A1Z5")] // Invalid state code (00)
    [InlineData("99AAAAA0000A1Z5")] // Invalid state code (99)
    public void Validate_WithInvalidFormat_ReturnsFailure(string? gstin)
    {
        var result = IndiaGstinValidator.Validate(gstin);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_RejectsInvalidChecksum()
    {
        // 22AAAAA0000A1Z0 - format is correct but checksum is wrong
        var result = IndiaGstinValidator.Validate("22AAAAA0000A1Z0");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("checksum", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_RemovesINPrefix()
    {
        // Test that IN prefix is stripped
        var result1 = IndiaGstinValidator.Validate("IN22AAAAA0000A1Z5");
        var result2 = IndiaGstinValidator.Validate("22AAAAA0000A1Z5");

        // Both should have same validity
        result1.IsValid.Should().Be(result2.IsValid);
    }

    [Fact]
    public void Validate_ChecksStateCode()
    {
        // State code must be 01-37
        var result1 = IndiaGstinValidator.Validate("00AAAAA0000A1Z5"); // Invalid: 00
        var result2 = IndiaGstinValidator.Validate("38AAAAA0000A1Z5"); // Invalid: 38

        result1.IsValid.Should().BeFalse();
        result2.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_ChecksPosition14IsZ()
    {
        // Position 14 must be 'Z'
        var result = IndiaGstinValidator.Validate("22AAAAA0000A1X5");
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidGstin_ReturnsNull(string? gstin)
    {
        var result = IndiaGstinValidator.GetVatDetails(gstin);
        result.Should().BeNull();
    }
}
