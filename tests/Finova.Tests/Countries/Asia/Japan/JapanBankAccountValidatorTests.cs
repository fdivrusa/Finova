using Finova.Core.Common;
using Finova.Countries.Asia.Japan.Validators;
using Xunit;

namespace Finova.Tests.Countries.Asia.Japan;

public class JapanBankAccountValidatorTests
{
    private readonly JapanBankAccountValidator _validator = new();

    [Theory]
    [InlineData("1234567")] // 7 digits
    [InlineData("12345678")] // 8 digits
    [InlineData("123-4567")] // With separators
    public void Validate_ValidInput_ReturnsSuccess(string input)
    {
        var result = _validator.Validate(input);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidInput, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("123456")] // Too short
    [InlineData("123456789")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidLength, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("12345A7")]
    public void Validate_NonDigits_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("1234567", "1234567")]
    [InlineData("123-4567", "1234567")]
    public void ParseBankAccount_ValidInput_ReturnsCorrectDetails(string input, string expectedCoreAccount)
    {
        var result = _validator.ParseBankAccount(input);

        Assert.NotNull(result);
        Assert.Equal("JP", result.CountryCode);
        Assert.Equal(expectedCoreAccount, result.CoreAccountNumber);
        Assert.Null(result.BranchCode);
        Assert.Null(result.CheckDigits);
    }
}
