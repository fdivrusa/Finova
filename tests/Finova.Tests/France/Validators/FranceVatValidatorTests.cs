using Finova.Countries.Europe.France.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.France.Validators;

public class FranceVatValidatorTests
{
    [Theory]
    [InlineData("FR61954506077")] // Google France
    [InlineData("FR89380129866")] // Microsoft France
    [InlineData("FR06403170701")] // Random valid (corrected checksum)
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = FranceVatValidator.ValidateFranceVat(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FR61954506070")] // Invalid checksum
    [InlineData("FR00000000000")] // All zeros
    [InlineData("FR123")] // Too short
    [InlineData("FR123456789012")] // Too long
    [InlineData("XX12345678901")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        // Act
        var result = FranceVatValidator.ValidateFranceVat(vat);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_WithValidVat_ReturnsDetails()
    {
        // Arrange
        var vat = "FR61954506077";

        // Act
        var details = FranceVatValidator.GetVatDetails(vat);

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeTrue();
        details.CountryCode.Should().Be("FR");
        details.Siren.Should().Be("954506077");
    }
}
