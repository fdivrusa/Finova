using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services.Global;

public class GlobalIdentityValidatorTests
{
    [Fact]
    public void ValidateNationalId_ShouldReturnTrue_ForValidChinaId()
    {
        // 11010519491231002X is a valid ID (checksum X)
        var result = GlobalIdentityValidator.ValidateNationalId("CN", "11010519491231002X");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateNationalId_ShouldReturnFalse_ForInvalidCountry()
    {
        var result = GlobalIdentityValidator.ValidateNationalId("XX", "123");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void ValidateTaxId_ShouldReturnTrue_ForValidUsEin()
    {
        // 12-3456789
        var result = GlobalIdentityValidator.ValidateTaxId("US", "12-3456789");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateTaxId_ShouldReturnTrue_ForValidAustraliaAbn()
    {
        // 51 824 753 556
        var result = GlobalIdentityValidator.ValidateTaxId("AU", "51 824 753 556");
        result.IsValid.Should().BeTrue();
    }
}
