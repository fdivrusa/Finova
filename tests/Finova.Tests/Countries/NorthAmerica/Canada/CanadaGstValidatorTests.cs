using Finova.Countries.NorthAmerica.Canada.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.Canada;

public class CanadaGstValidatorTests
{
    // Note: Canadian GST/HST numbers use the 9-digit BN with Luhn checksum validation

    [Fact]
    public void Validate_WithInvalidBnChecksum_ReturnsFailure()
    {
        // 123456789 has invalid Luhn checksum
        var result = CanadaGstValidator.Validate("123456789RT0001");
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456789")] // BN only, no RT
    [InlineData("123456789RT")] // Missing account number
    [InlineData("12345678RT0001")] // Too short BN (8 digits)
    [InlineData("1234567890RT0001")] // Too long BN (10 digits)
    public void Validate_WithInvalidGst_ReturnsFailure(string? gst)
    {
        var result = CanadaGstValidator.Validate(gst);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123456789")] // BN only
    public void GetVatDetails_WithInvalidGst_ReturnsNull(string? gst)
    {
        var result = CanadaGstValidator.GetVatDetails(gst);
        result.Should().BeNull();
    }

    [Fact]
    public void Validate_ChecksFormat_ReturnsFailureForMissingRT()
    {
        // Valid length but missing RT suffix
        var result = CanadaGstValidator.Validate("123456789AB0001");
        result.IsValid.Should().BeFalse();
    }
}
