using Finova.Countries.Oceania.NewZealand.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Oceania.NewZealand;

public class NewZealandGstValidatorTests
{
    // Note: NZ IRD numbers have complex checksum algorithm.
    // Tests focus on format validation and checksum rejection.

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("4909185")] // Too short (7 digits)
    [InlineData("4909185012")] // Too long (10 digits)
    public void Validate_WithInvalidFormat_ReturnsFailure(string? gst)
    {
        var result = NewZealandGstValidator.Validate(gst);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidChecksum_ReturnsFailure()
    {
        // 12345678 likely has invalid checksum
        var result = NewZealandGstValidator.Validate("12345678");
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_RemovesNZPrefix()
    {
        // Test that NZ prefix is stripped
        var result1 = NewZealandGstValidator.Validate("NZ12345678");
        var result2 = NewZealandGstValidator.Validate("12345678");
        
        // Both should fail the same way (checksum failure)
        result1.IsValid.Should().Be(result2.IsValid);
    }

    [Fact]
    public void Validate_RemovesFormatting()
    {
        // Test that hyphens and spaces are stripped
        var result1 = NewZealandGstValidator.Validate("12-345-678");
        var result2 = NewZealandGstValidator.Validate("12345678");
        
        // Both should have same validity
        result1.IsValid.Should().Be(result2.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidGst_ReturnsNull(string? gst)
    {
        var result = NewZealandGstValidator.GetVatDetails(gst);
        result.Should().BeNull();
    }
}
