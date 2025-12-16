using Finova.Services;
using Xunit;

namespace Finova.Tests.Services;

public class EuropeIdentifierRoutingTests
{
    [Theory]
    [InlineData("BE", true, true, IdentifierKind.Vat)]
    [InlineData("FR", true, true, IdentifierKind.Vat)]
    [InlineData("CH", true, true, IdentifierKind.Vat)] // VAT supported, but not EU/VIES
    [InlineData("GB", true, true, IdentifierKind.Vat)]
    [InlineData("TR", true, true, IdentifierKind.BusinessTaxId)] // VAT supported (as VKN), but it's a Tax ID
    [InlineData("VA", true, true, IdentifierKind.InvoicingSchemeIdentifier)] // VAT supported (as invoicing id), not Enterprise
    [InlineData("US", false, false, IdentifierKind.NotApplicable)] // Unknown
    public void GetSupport_ReturnsCorrectMetadata(
        string countryCode,
        bool supportsVat,
        bool supportsEnterprise,
        IdentifierKind expectedKind)
    {
        // Act
        var support = EuropeIdentifierRouting.GetSupport(countryCode);

        // Assert
        Assert.Equal(supportsVat, support.SupportsVatValidation);
        Assert.Equal(supportsEnterprise, support.SupportsEnterpriseValidation);
        Assert.Equal(expectedKind, support.VatKind);
    }

    [Fact]
    public void GetVatDetails_EnrichesMetadata_Belgium()
    {
        // Arrange
        // BE VAT is Mod97. BE0202239951 is valid (Proximus).
        var details = EuropeVatValidator.GetVatDetails("BE0202239951");

        // Assert
        Assert.NotNull(details);
        Assert.True(details.IsEuVat);
        Assert.True(details.IsViesEligible);
        Assert.Equal("Vat", details.IdentifierKind);
    }

    [Fact]
    public void GetVatDetails_EnrichesMetadata_Switzerland()
    {
        // Arrange
        // CH VAT: CHE-123.456.788 MWST
        // Valid: CHE-107.787.577 (Google Switzerland)
        var details = EuropeVatValidator.GetVatDetails("CHE107787577");

        // Assert
        Assert.NotNull(details);
        Assert.False(details.IsEuVat);
        Assert.False(details.IsViesEligible);
        Assert.Equal("Vat", details.IdentifierKind);
    }

    [Fact]
    public void GetVatDetails_EnrichesMetadata_Turkey()
    {
        // Arrange
        // TR VKN is 10 digits.
        // 1234567890
        var details = EuropeVatValidator.GetVatDetails("TR1234567890");

        // Assert
        Assert.NotNull(details);
        Assert.False(details.IsEuVat);
        Assert.False(details.IsViesEligible);
        Assert.Equal("BusinessTaxId", details.IdentifierKind);
    }

    [Fact]
    public void GetVatDetails_EnrichesMetadata_Vatican()
    {
        // Arrange
        // VA VAT is 11 digits (Italian format).
        // VA00462350018
        var details = EuropeVatValidator.GetVatDetails("VA00462350018");

        // Assert
        Assert.NotNull(details);
        Assert.False(details.IsEuVat);
        Assert.False(details.IsViesEligible);
        Assert.Equal("InvoicingSchemeIdentifier", details.IdentifierKind);
    }
}
