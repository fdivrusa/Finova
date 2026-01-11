using Finova.Core.Common;
using Finova.Countries.Asia.Pakistan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Pakistan.Validators;

public class PakistanBbanValidatorTests
{
    [Theory]
    [InlineData("SCBL0000001123456702")] // Valid - from PK36SCBL0000001123456702
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = PakistanBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = PakistanBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                      // Invalid Length - too short
    [InlineData("SCBL000000112345670")]      // Invalid Length - 19 chars (missing one)
    [InlineData("SCBL00000011234567020")]    // Invalid Length - 21 chars (one extra)
    [InlineData("1CBL0000001123456702")]     // Invalid Format - bank code must be 4 letters
    [InlineData("SCBL000000112345670A")]     // Invalid Format - account must be digits
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = PakistanBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("SCBL0000001123456702", "SCBL", "0000001123456702")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new PakistanBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("PK");
    }
}
