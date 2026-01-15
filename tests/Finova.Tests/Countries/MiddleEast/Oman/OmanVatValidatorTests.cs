using Finova.Countries.MiddleEast.Oman.Validators;
using Finova.Core.Common;
using Finova.Core.Vat;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Oman;

public class OmanVatValidatorTests
{
    [Theory]
    [InlineData("OM1234567890123")] // Valid 15 chars (OM + 13 digits)
    [InlineData("123456789012345")] // Valid 15 digits
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = OmanVatValidator.ValidateVat(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")] // Too short
    [InlineData("1234567890123456")] // Too long (16)
    [InlineData("OM123456789012")] // Short with prefix (14 total)
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = OmanVatValidator.ValidateVat(vat);
        result.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public void Validate_WithInvalidLength_ReturnsCorrectErrorMessage()
    {
         var result = OmanVatValidator.ValidateVat("123");
         result.IsValid.Should().BeFalse();
         result.Errors.Should().ContainSingle(e => e.Message == ValidationMessages.InvalidOmanVatLength);
    }
    
    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var vat = "OM1234567890123";
        IVatValidator validator = new OmanVatValidator();
        var result = validator.Parse(vat);

        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("OM");
        result.IdentifierKind.Should().Be("VAT");
        result.IsValid.Should().BeTrue();
    }
}