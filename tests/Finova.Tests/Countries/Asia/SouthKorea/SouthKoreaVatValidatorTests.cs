using Finova.Countries.Asia.SouthKorea.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.SouthKorea;

public class SouthKoreaVatValidatorTests
{
    [Fact]
    public void Validate_WithValidBrn_ReturnsSuccess()
    {
        // Valid BRN with correct checksum
        // Format: 10 digits, weights: 1,3,7,1,3,7,1,3,5
        // Example: 1234567893 
        // Calculation: 1*1 + 2*3 + 3*7 + 4*1 + 5*3 + 6*7 + 7*1 + 8*3 + 9*5 + floor((9*5)/10)
        //           = 1 + 6 + 21 + 4 + 15 + 42 + 7 + 24 + 45 + 4 = 169
        // Check: (10 - 169%10) % 10 = (10 - 9) % 10 = 1
        // So last digit should be 1 -> 1234567891
        var result = SouthKoreaVatValidator.Validate("1234567891");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithKRPrefixedValidBrn_ReturnsSuccess()
    {
        var result = SouthKoreaVatValidator.Validate("KR1234567891");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456789")] // Too short (9 digits)
    [InlineData("12345678901")] // Too long (11 digits)
    [InlineData("ABCDEFGHIJ")] // Non-numeric
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = SouthKoreaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidChecksum_ReturnsFailure()
    {
        // 1234567890 has invalid checksum (should be 1)
        var result = SouthKoreaVatValidator.Validate("1234567890");
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetVatDetails_ReturnsCorrectProperties()
    {
        var result = SouthKoreaVatValidator.GetVatDetails("1234567891");
        
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("KR");
        result.IdentifierKind.Should().Be("BRN");
        result.IsEuVat.Should().BeFalse();
        result.Notes.Should().Contain("사업자등록번호");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("12345")] // Too short
    public void GetVatDetails_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = SouthKoreaVatValidator.GetVatDetails(vat);
        result.Should().BeNull();
    }
}
