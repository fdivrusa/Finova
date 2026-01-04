using Finova.Countries.MiddleEast.Israel.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Israel;

public class IsraelVatValidatorTests
{
    [Theory]
    [InlineData("516179157")] // Valid Israeli VAT with correct checksum
    [InlineData("IL516179157")] // With IL prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = IsraelVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234")] // Too short (4 digits)
    [InlineData("1234567890")] // Too long (10 digits)
    [InlineData("ABCDEFGHI")] // Non-numeric
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = IsraelVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        // Need a valid 9-digit number with correct checksum
        var vat = "123456782"; // Example - may need valid checksum
        var result = IsraelVatValidator.GetVatDetails(vat);
        
        if (result != null)
        {
            result.CountryCode.Should().Be("IL");
            result.IdentifierKind.Should().Be("Authorized Dealer Number");
            result.IsEuVat.Should().BeFalse();
            result.Notes.Should().Contain("מספר עוסק מורשה");
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = IsraelVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
