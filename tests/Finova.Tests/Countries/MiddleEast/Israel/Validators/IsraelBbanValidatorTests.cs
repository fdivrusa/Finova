using Finova.Core.Common;
using Finova.Countries.MiddleEast.Israel.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Israel.Validators;

public class IsraelBbanValidatorTests
{
    [Theory]
    [InlineData("0108000000099999999")] // Valid - from IL620108000000099999999
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = IsraelBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = IsraelBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                   // Invalid Length - too short
    [InlineData("010800000009999999")]    // Invalid Length - 18 digits (missing one)
    [InlineData("01080000000999999999")]  // Invalid Length - 20 digits (one extra)
    [InlineData("010800000009999999A")]   // Invalid Format - contains letter
    [InlineData("A108000000099999999")]   // Invalid Format - starts with letter
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = IsraelBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("0108000000099999999", "010", "800", "0000099999999")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedBranchCode, string expectedAccount)
    {
        // Arrange
        var validator = new IsraelBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.BranchCode.Should().Be(expectedBranchCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("IL");
    }
}
