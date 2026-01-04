using Finova.Countries.Asia.Singapore.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Singapore;

public class SingaporeGstValidatorTests
{
    [Theory]
    [InlineData("200312345A")] // Valid UEN format
    [InlineData("SG200312345A")] // With SG prefix
    [InlineData("52044531M")] // Valid business UEN
    [InlineData("T08LL0001A")] // Valid local company UEN
    public void Validate_WithValidGst_ReturnsSuccess(string gst)
    {
        var result = SingaporeGstValidator.Validate(gst);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("200312345A-GST")] // With GST suffix
    [InlineData("SG200312345A-GST")] // With prefix and suffix
    public void Validate_WithGstSuffix_ReturnsSuccess(string gst)
    {
        var result = SingaporeGstValidator.Validate(gst);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("2003123")] // Too short
    [InlineData("ABCDEFGHIJK")] // Invalid format
    public void Validate_WithInvalidGst_ReturnsFailure(string? gst)
    {
        var result = SingaporeGstValidator.Validate(gst);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("200312345A", "SG", "GST")]
    [InlineData("SG200312345A", "SG", "GST")]
    public void GetVatDetails_WithValidGst_ReturnsDetails(string gst, string expectedCountryCode, string expectedKind)
    {
        var result = SingaporeGstValidator.GetVatDetails(gst);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.IdentifierKind.Should().Be(expectedKind);
        result.IsEuVat.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("INVALID")] // Invalid format
    public void GetVatDetails_WithInvalidGst_ReturnsNull(string? gst)
    {
        var result = SingaporeGstValidator.GetVatDetails(gst);
        result.Should().BeNull();
    }
}
