using Finova.Core.Common;
using Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.VirginIslandsBritish.Validators;

public class VirginIslandsBritishBbanValidatorTests
{
    [Theory]
    [InlineData("VPVG0000012345678901")] // Valid - from VG96VPVG0000012345678901
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = VirginIslandsBritishBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = VirginIslandsBritishBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                        // Invalid Length - too short
    [InlineData("VPVG000001234567890")]        // Invalid Length - 19 chars (missing one)
    [InlineData("VPVG00000123456789010")]      // Invalid Length - 21 chars (one extra)
    [InlineData("1PVG0000012345678901")]       // Invalid Format - bank code must be 4 letters
    [InlineData("VPVG000001234567890A")]       // Invalid Format - account must be digits
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = VirginIslandsBritishBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("VPVG0000012345678901", "VPVG", "0000012345678901")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new VirginIslandsBritishBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("VG");
    }
}
