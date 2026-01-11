using Finova.Core.Common;
using Finova.Countries.MiddleEast.Jordan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Jordan.Validators;

public class JordanBbanValidatorTests
{
    [Theory]
    [InlineData("CBJO0010000000000131000302")] // Valid - from JO94CBJO0010000000000131000302
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = JordanBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = JordanBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                           // Invalid Length - too short
    [InlineData("CBJO001000000000013100030")]     // Invalid Length - 25 chars (missing one)
    [InlineData("CBJO00100000000001310003020")]   // Invalid Length - 27 chars (one extra)
    [InlineData("1BJO0010000000000131000302")]    // Invalid Format - bank code must start with letters
    [InlineData("CB1O0010000000000131000302")]    // Invalid Format - bank code must be 4 letters
    [InlineData("CBJOABCD000000000131000302")]    // Invalid Format - branch code must be digits
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = JordanBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("CBJO0010000000000131000302", "CBJO", "0010", "000000000131000302")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedBranchCode, string expectedAccount)
    {
        // Arrange
        var validator = new JordanBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.BranchCode.Should().Be(expectedBranchCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("JO");
    }
}
