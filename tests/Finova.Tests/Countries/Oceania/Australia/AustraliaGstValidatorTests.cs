using Finova.Countries.Oceania.Australia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Oceania.Australia;

public class AustraliaGstValidatorTests
{
    [Theory]
    [InlineData("51824753556")] // Valid ABN
    [InlineData("AU51824753556")] // With AU prefix
    [InlineData("53004085616")] // ANZ ABN
    [InlineData("51 824 753 556")] // With spaces
    public void Validate_WithValidGst_ReturnsSuccess(string gst)
    {
        var result = AustraliaGstValidator.Validate(gst);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("5182475355")] // Too short (10 digits)
    [InlineData("518247535567")] // Too long (12 digits)
    [InlineData("51824753557")] // Invalid checksum
    [InlineData("00000000000")] // All zeros
    public void Validate_WithInvalidGst_ReturnsFailure(string? gst)
    {
        var result = AustraliaGstValidator.Validate(gst);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("51824753556", "AU", "GST")]
    [InlineData("AU51824753556", "AU", "GST")]
    public void GetVatDetails_WithValidGst_ReturnsDetails(string gst, string expectedCountryCode, string expectedKind)
    {
        var result = AustraliaGstValidator.GetVatDetails(gst);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.IdentifierKind.Should().Be(expectedKind);
        result.IsEuVat.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("51824753557")] // Invalid checksum
    public void GetVatDetails_WithInvalidGst_ReturnsNull(string? gst)
    {
        var result = AustraliaGstValidator.GetVatDetails(gst);
        result.Should().BeNull();
    }
}
