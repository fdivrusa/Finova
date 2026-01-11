using Finova.Core.Common;
using Finova.Countries.MiddleEast.Bahrain.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.Bahrain.Validators;

public class BahrainBbanValidatorTests
{
    [Theory]
    [InlineData("BMAG00001299123456")] // Valid - from BH67BMAG00001299123456
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = BahrainBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = BahrainBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                    // Invalid Length - too short
    [InlineData("BMAG0000129912345")]      // Invalid Length - 17 chars (missing one)
    [InlineData("BMAG000012991234567")]    // Invalid Length - 19 chars (one extra)
    [InlineData("1MAG00001299123456")]     // Invalid Format - bank code must start with letters
    [InlineData("BM1G00001299123456")]     // Invalid Format - bank code must be 4 letters
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = BahrainBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("BMAG00001299123456", "BMAG", "00001299123456")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new BahrainBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("BH");
    }
}
