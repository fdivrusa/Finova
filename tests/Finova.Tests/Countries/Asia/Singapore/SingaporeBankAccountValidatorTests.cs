using Finova.Countries.Asia.Singapore.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Asia.Singapore;

public class SingaporeBankAccountValidatorTests
{
    private readonly SingaporeBankAccountValidator _validator = new();

    [Theory]
    [InlineData("1234567890")] // 10 digits
    [InlineData("123-456-7890")] // With separators
    [InlineData("1234567")] // 7 digits (min)
    [InlineData("12345678901234567")] // 17 digits (max)
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
    [InlineData("123456789012345678")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidLength, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("12345A7890")]
    [InlineData("12345!7890")]
    public void Validate_NonDigits_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.InvalidFormat, result.Errors[0].Code);
    }

    [Theory]
    [InlineData("1234567890", "123", "4567890")] // 10 digits -> Branch(3) + Account(7)
    [InlineData("123-456-7890", "123", "4567890")] // With separators
    [InlineData("1234567", null, "1234567")] // < 10 digits -> No Branch
    public void ParseBankAccount_ValidInput_ReturnsCorrectDetails(string input, string? expectedBranch, string expectedCoreAccount)
    {
        var result = _validator.ParseBankAccount(input);
        
        Assert.NotNull(result);
        Assert.Equal("SG", result.CountryCode);
        Assert.Equal(expectedCoreAccount, result.CoreAccountNumber);
        Assert.Equal(expectedBranch, result.BranchCode);
        Assert.Null(result.CheckDigits);
    }
}
