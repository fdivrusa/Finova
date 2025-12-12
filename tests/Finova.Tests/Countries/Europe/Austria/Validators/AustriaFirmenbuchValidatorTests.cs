using Finova.Countries.Europe.Austria.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.Austria.Validators;

public class AustriaFirmenbuchValidatorTests
{
    [Theory]
    [InlineData("123456x")]
    [InlineData("FN 123456x")]
    [InlineData("123x")]
    public void Validate_ShouldReturnSuccess_ForValidFormat(string number)
    {
        var result = AustriaFirmenbuchValidator.ValidateFirmenbuch(number);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("1234567x")] // Too many digits
    [InlineData("123456")] // Missing letter
    [InlineData("x")] // Missing digits
    public void Validate_ShouldReturnFailure_ForInvalidFormat(string number)
    {
        var result = AustriaFirmenbuchValidator.ValidateFirmenbuch(number);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.Errors[0].Code);
    }
}

