using Finova.Core.Common;
using Finova.Countries.Asia.Kazakhstan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Kazakhstan.Validators;

public class KazakhstanBbanValidatorTests
{
    [Theory]
    [InlineData("125KZT5004100100")] // Valid - from KZ86125KZT5004100100
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = KazakhstanBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = KazakhstanBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                   // Invalid Length - too short
    [InlineData("125KZT500410010")]       // Invalid Length - 15 chars (missing one)
    [InlineData("125KZT50041001000")]     // Invalid Length - 17 chars (one extra)
    [InlineData("A25KZT5004100100")]      // Invalid Format - bank code must be 3 digits
    [InlineData("12AKZT5004100100")]      // Invalid Format - bank code must be 3 digits
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = KazakhstanBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("125KZT5004100100", "125", "KZT5004100100")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new KazakhstanBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("KZ");
    }
}
