using Finova.Core.Common;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Brazil.Validators;

public class BrazilBbanValidatorTests
{
    [Theory]
    [InlineData("00360305000010009795493C1")] // Valid - from BR1800360305000010009795493C1
    [InlineData("00000000000010932840814P2")] // Valid - from BR1500000000000010932840814P2
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = BrazilBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = BrazilBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                          // Invalid Length - too short
    [InlineData("0036030500001000979549C1")]     // Invalid Length - 24 chars (missing one)
    [InlineData("003603050000100097954993C1")]   // Invalid Length - 26 chars (one extra)
    [InlineData("A0360305000010009795493C1")]    // Invalid Format - bank code must be digits
    [InlineData("003603050000100097954931P")]    // Invalid Format - account type (position 24) must be letter
    [InlineData("0036030500001000979549312")]    // Invalid Format - owner indicator (position 25) must be letter
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = BrazilBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("00360305000010009795493C1", "00360305", "00001", "0009795493")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedBranchCode, string expectedAccount)
    {
        // Arrange
        var validator = new BrazilBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.BranchCode.Should().Be(expectedBranchCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("BR");
    }
}
