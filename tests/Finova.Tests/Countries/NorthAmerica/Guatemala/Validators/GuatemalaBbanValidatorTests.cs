using Finova.Core.Common;
using Finova.Countries.NorthAmerica.Guatemala.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.Guatemala.Validators;

public class GuatemalaBbanValidatorTests
{
    [Theory]
    [InlineData("TRAJ01020000001210029690")] // Valid - from GT82TRAJ01020000001210029690
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = GuatemalaBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = GuatemalaBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                          // Invalid Length - too short
    [InlineData("TRAJ0102000000121002969")]      // Invalid Length - 23 chars (missing one)
    [InlineData("TRAJ0102000000121002969099")]   // Invalid Length - 25 chars (one extra)
    [InlineData("1RAJ01020000001210029690")]     // Invalid Format - bank code must be 4 letters
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = GuatemalaBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("TRAJ01020000001210029690", "TRAJ", "01020000001210029690")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new GuatemalaBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("GT");
    }
}
