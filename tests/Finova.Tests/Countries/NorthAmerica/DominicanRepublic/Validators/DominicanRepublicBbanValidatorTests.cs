using Finova.Core.Common;
using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.DominicanRepublic.Validators;

public class DominicanRepublicBbanValidatorTests
{
    [Theory]
    [InlineData("ACAU00000000000123456789")] // Valid - from DO22ACAU00000000000123456789
    [InlineData("BAGR00000001212453611324")] // Valid - from DO28BAGR00000001212453611324
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = DominicanRepublicBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = DominicanRepublicBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                          // Invalid Length - too short
    [InlineData("ACAU0000000000012345678")]      // Invalid Length - 23 chars (missing one)
    [InlineData("ACAU0000000000012345678901")]   // Invalid Length - 25 chars (one extra)
    [InlineData("1CAU00000000000123456789")]     // Invalid Format - bank code must be 4 letters
    [InlineData("ACAU0000000000012345678A")]     // Invalid Format - account must be digits
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = DominicanRepublicBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("ACAU00000000000123456789", "ACAU", "00000000000123456789")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new DominicanRepublicBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("DO");
    }
}
