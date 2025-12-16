using Finova.Core.Common;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Brazil;

public class BrazilBankCodeValidatorTests
{
    private readonly BrazilBankCodeValidator _validator = new();

    [Theory]
    [InlineData("001")]
    [InlineData("237")]
    public void Validate_ValidBankCode_ReturnsSuccess(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("01")] // Too short
    [InlineData("0001")] // Too long
    [InlineData("A01")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("001", "001")]
    [InlineData("237", "237")]
    public void ParseRoutingNumber_ValidInput_ReturnsDetails(string input, string expectedBank)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("BR");
        result.BankCode.Should().Be(expectedBank);
    }
}
