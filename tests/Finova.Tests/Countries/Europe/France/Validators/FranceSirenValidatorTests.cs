using Finova.Core.Common;
using Finova.Countries.Europe.France.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.France.Validators;

public class FranceSirenValidatorTests
{
    [Theory]
    [InlineData("732 829 320")] // Valid SIREN (BNP Paribas)
    [InlineData("732829320")]   // Valid SIREN without spaces
    [InlineData("732.829.320")] // Valid SIREN with dots
    public void Validate_ValidSiren_ReturnsSuccess(string siren)
    {
        var result = FranceSirenValidator.ValidateSiren(siren);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Validate_EmptySiren_ReturnsFailure(string? siren)
    {
        var result = FranceSirenValidator.ValidateSiren(siren);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("12345678")]   // Too short
    [InlineData("1234567890")] // Too long
    [InlineData("ABCDEFGHI")]  // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string siren)
    {
        var result = FranceSirenValidator.ValidateSiren(siren);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("123456789")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string siren)
    {
        var result = FranceSirenValidator.ValidateSiren(siren);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidChecksum, result.Errors[0].Code);
    }
}

