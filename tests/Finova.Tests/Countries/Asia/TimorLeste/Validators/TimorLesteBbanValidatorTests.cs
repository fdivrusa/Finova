using Finova.Core.Common;
using Finova.Countries.Asia.TimorLeste.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.TimorLeste.Validators;

public class TimorLesteBbanValidatorTests
{
    [Theory]
    [InlineData("0080012345678910157")] // Valid - from TL380080012345678910157
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = TimorLesteBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = TimorLesteBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                     // Invalid Length - too short
    [InlineData("008001234567891015")]      // Invalid Length - 18 digits (missing one)
    [InlineData("00800123456789101570")]    // Invalid Length - 20 digits (one extra)
    [InlineData("008001234567891015A")]     // Invalid Format - must be digits only
    [InlineData("A080012345678910157")]     // Invalid Format - starts with letter
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = TimorLesteBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("0080012345678910157", "008", "00123456789101", "57")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount, string expectedCheckDigits)
    {
        // Arrange
        var validator = new TimorLesteBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.NationalCheckDigits.Should().Be(expectedCheckDigits);
        result.CountryCode.Should().Be("TL");
    }
}
