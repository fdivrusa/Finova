using Finova.Core.Common;
using Finova.Countries.Asia.Singapore.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Singapore;

public class SingaporeBankCodeValidatorTests
{
    private readonly SingaporeBankCodeValidator _validator = new();

    [Theory]
    [InlineData("7171")] // Bank code only (4 digits)
    [InlineData("001")] // Branch code only (3 digits)
    [InlineData("7171001")] // Combined bank + branch (7 digits)
    [InlineData("1234567")] // Another combined format
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
    [InlineData("01")] // Too short (2 digits)
    [InlineData("71710")] // Invalid length (5 digits)
    [InlineData("717100")] // Invalid length (6 digits)
    [InlineData("71710011")] // Too long (8 digits)
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("717A")] // Contains letter
    [InlineData("AB01")] // Contains letters
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Fact]
    public void ParseRoutingNumber_CombinedFormat_ReturnsDetails()
    {
        // Arrange
        var bankCode = "7171001";

        // Act
        var result = _validator.ParseRoutingNumber(bankCode);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("SG");
        result.BankCode.Should().Be("7171");
        result.BranchCode.Should().Be("001");
    }

    [Fact]
    public void ParseRoutingNumber_BankCodeOnly_ReturnsDetails()
    {
        // Arrange
        var bankCode = "7171";

        // Act
        var result = _validator.ParseRoutingNumber(bankCode);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("SG");
        result.BankCode.Should().Be("7171");
        result.BranchCode.Should().BeNull();
    }

    [Fact]
    public void ParseRoutingNumber_BranchCodeOnly_ReturnsDetails()
    {
        // Arrange
        var bankCode = "001";

        // Act
        var result = _validator.ParseRoutingNumber(bankCode);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("SG");
        result.BankCode.Should().BeNull();
        result.BranchCode.Should().Be("001");
    }

    [Fact]
    public void Parse_ValidBankCode_ReturnsNormalizedValue()
    {
        // Arrange
        var bankCode = "7171-001";

        // Act
        var result = _validator.Parse(bankCode);

        // Assert
        result.Should().Be("7171001");
    }

    [Fact]
    public void Parse_InvalidBankCode_ReturnsNull()
    {
        // Arrange
        var bankCode = "invalid";

        // Act
        var result = _validator.Parse(bankCode);

        // Assert
        result.Should().BeNull();
    }
}
