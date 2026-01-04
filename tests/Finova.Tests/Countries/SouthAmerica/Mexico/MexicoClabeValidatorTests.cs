using Finova.Core.Common;
using Finova.Countries.SouthAmerica.Mexico.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Mexico;

public class MexicoClabeValidatorTests
{
    private readonly MexicoClabeValidator _validator = new();

    [Theory]
    [InlineData("002010077777777771")] // Valid CLABE with correct check digit
    [InlineData("032180000118359719")] // Another valid CLABE
    public void Validate_ValidClabe_ReturnsSuccess(string input)
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
    [InlineData("00201007777777777")] // Too short (17 digits)
    [InlineData("0020100777777777710")] // Too long (19 digits)
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("00201007777777777A")] // Contains letter
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("002010077777777770")] // Wrong check digit (should be 1)
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void ParseRoutingNumber_ValidClabe_ReturnsDetails()
    {
        // Arrange
        var clabe = "002010077777777771";

        // Act
        var result = _validator.ParseRoutingNumber(clabe);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("MX");
        result.BankCode.Should().Be("002");
        result.BranchCode.Should().Be("010");
        result.CheckDigits.Should().Be("1");
    }

    [Fact]
    public void ParseBankAccount_ValidClabe_ReturnsDetails()
    {
        // Arrange
        var clabe = "002010077777777771";

        // Act
        var result = _validator.ParseBankAccount(clabe);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("MX");
        result.BranchCode.Should().Be("010");
        result.CoreAccountNumber.Should().Be("07777777777");
        result.CheckDigits.Should().Be("1");
    }

    [Fact]
    public void Parse_ValidClabe_ReturnsNormalizedValue()
    {
        // Arrange
        var clabe = "002-010-077777777771";

        // Act
        var result = _validator.Parse(clabe);

        // Assert
        result.Should().Be("002010077777777771");
    }

    [Fact]
    public void Parse_InvalidClabe_ReturnsNull()
    {
        // Arrange
        var clabe = "invalid";

        // Act
        var result = _validator.Parse(clabe);

        // Assert
        result.Should().BeNull();
    }
}
