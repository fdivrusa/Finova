using Finova.Core.Common;
using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.ElSalvador.Validators;

public class ElSalvadorBbanValidatorTests
{
    [Theory]
    [InlineData("CENR00000000000000700025")] // Valid - from SV62CENR00000000000000700025
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = ElSalvadorBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = ElSalvadorBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                          // Invalid Length - too short
    [InlineData("CENR0000000000000070002")]      // Invalid Length - 23 chars (missing one)
    [InlineData("CENR0000000000000070002599")]   // Invalid Length - 25 chars (one extra)
    [InlineData("1ENR00000000000000700025")]     // Invalid Format - bank code must be 4 letters
    [InlineData("CENR0000000000000070002A")]     // Invalid Format - account must be digits
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = ElSalvadorBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("CENR00000000000000700025", "CENR", "00000000000000700025")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new ElSalvadorBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("SV");
    }
}
