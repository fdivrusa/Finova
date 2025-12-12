using Finova.Core.Common;
using Finova.Countries.Europe.Sweden.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Sweden.Validators;

public class SwedenIbanValidatorTests
{
    private readonly SwedenIbanValidator _validator;

    public SwedenIbanValidatorTests()
    {
        _validator = new SwedenIbanValidator();
    }

    #region Structure Tests

    [Fact]
    public void Validate_WithNullIban_ReturnsInvalidInput()
    {
        var result = _validator.Validate(null);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void Validate_WithEmptyIban_ReturnsInvalidInput()
    {
        var result = _validator.Validate("");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void Validate_WithInvalidLength_ReturnsInvalidLength()
    {
        // SE + 22 digits = 24 chars required. 
        var result = _validator.Validate("SE123456789012345678901"); // 23 chars
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
    }

    [Fact]
    public void Validate_WithInvalidCountryCode_ReturnsInvalidCountryCode()
    {
        var result = _validator.Validate("DE1234567890123456789012"); // 24 chars but DE
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidCountryCode);
    }

    [Fact]
    public void Validate_WithNonDigitsInBody_ReturnsInvalidFormat()
    {
        var result = _validator.Validate("SE123456789012345678901X");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Fact]
    public void Validate_WithInvalidMod97Checksum_ReturnsInvalidChecksum()
    {
        // SE45 5000 0000 0583 9825 7466 is valid. Change check digits to 00.
        var result = _validator.Validate("SE0050000000058398257466");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidChecksum);
    }

    #endregion
}
