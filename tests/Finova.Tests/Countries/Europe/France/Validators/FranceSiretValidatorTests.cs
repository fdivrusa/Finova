using Finova.Countries.Europe.France.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.France.Validators;

public class FranceSiretValidatorTests
{
    [Theory]
    [InlineData("73282932000074")] // Valid SIRET (Google France)
    public void Validate_ShouldReturnSuccess_ForValidSiret(string number)
    {
        var result = FranceSiretValidator.ValidateSiret(number);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("12345678901234")] // Invalid Checksum
    [InlineData("123")] // Too short
    public void Validate_ShouldReturnFailure_ForInvalidSiret(string number)
    {
        var result = FranceSiretValidator.ValidateSiret(number);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void GetSiretDetails_ValidSiret_ReturnsDetails()
    {
        var siret = "732 829 320 00074";
        var details = FranceSiretValidator.GetSiretDetails(siret);

        Assert.NotNull(details);
        Assert.True(details.IsValid);
        Assert.Equal("73282932000074", details.Siret);
        Assert.Equal("732829320", details.Siren);
        Assert.Equal("00074", details.Nic);
    }

    [Fact]
    public void GetSiretDetails_InvalidSiret_ReturnsNull()
    {
        var details = FranceSiretValidator.GetSiretDetails("invalid");
        Assert.Null(details);
    }
}

