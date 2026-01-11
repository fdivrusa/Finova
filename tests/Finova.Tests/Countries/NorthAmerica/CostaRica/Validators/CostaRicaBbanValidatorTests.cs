using Finova.Core.Common;
using Finova.Countries.NorthAmerica.CostaRica.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.CostaRica.Validators;

public class CostaRicaBbanValidatorTests
{
    [Theory]
    [InlineData("015202001026284066")] // Valid - from CR05015202001026284066
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = CostaRicaBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = CostaRicaBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("ABC")]                     // Invalid Length - too short
    [InlineData("01520200102628406")]       // Invalid Length - 17 digits (missing one)
    [InlineData("0152020010262840660")]     // Invalid Length - 19 digits (one extra)
    [InlineData("115202001026284066")]      // Invalid Format - reserve digit must be 0
    [InlineData("A15202001026284066")]      // Invalid Format - must be digits only
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = CostaRicaBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("015202001026284066", "152", "02001026284066")]
    public void ParseDetails_ShouldExtractCorrectComponents(string bban, string expectedBankCode, string expectedAccount)
    {
        // Arrange
        var validator = new CostaRicaBbanValidator();

        // Act
        var result = validator.ParseDetails(bban);

        // Assert
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(expectedBankCode);
        result.AccountNumber.Should().Be(expectedAccount);
        result.CountryCode.Should().Be("CR");
    }
}
