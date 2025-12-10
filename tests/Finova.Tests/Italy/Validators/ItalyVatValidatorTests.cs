using Finova.Countries.Europe.Italy.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Italy.Validators;

public class ItalyVatValidatorTests
{
    [Theory]
    [InlineData("IT06363391001")] // Enel
    [InlineData("IT00905811006")] // Eni
    [InlineData("IT00488410010")] // Telecom Italia
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        // Act
        var result = ItalyVatValidator.ValidateItalyVat(vat);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IT06363391000")] // Invalid checksum
    [InlineData("IT00000000000")] // All zeros (technically valid Luhn but likely invalid VAT, but for now we check algo)
    // Actually 00000000000 passes Luhn (0 sum). But usually progressive number can't be 0.
    // Let's stick to checksum validation for now.
    [InlineData("IT123")] // Too short
    [InlineData("IT123456789012")] // Too long
    [InlineData("XX12345678901")] // Wrong prefix
    public void Validate_WithInvalidVat_ReturnsFailure(string vat)
    {
        // Act
        var result = ItalyVatValidator.ValidateItalyVat(vat);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_WithValidVat_ReturnsDetails()
    {
        // Arrange
        var vat = "IT06363391001";

        // Act
        var details = ItalyVatValidator.GetVatDetails(vat);

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeTrue();
        details.CountryCode.Should().Be("IT");
        details.OfficeCode.Should().Be("100"); // 8-10th digits: 100
    }
}
