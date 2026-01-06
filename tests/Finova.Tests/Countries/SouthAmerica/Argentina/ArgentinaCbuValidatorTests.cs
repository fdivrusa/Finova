using Finova.Core.Common;
using Finova.Countries.SouthAmerica.Argentina.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Argentina;

public class ArgentinaCbuValidatorTests
{
    private readonly ArgentinaCbuValidator _validator = new();

    [Theory]
    [InlineData("2850590940090418135201")] // Valid CBU
    [InlineData("0110599520000000000103")] // Another valid CBU format
    public void Validate_ValidCbu_ReturnsSuccess(string input)
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
    [InlineData("285059094009041813520")] // Too short (21 digits)
    [InlineData("28505909400904181352010")] // Too long (23 digits)
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("285059094009041813520A")] // Contains letter
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("2850590940090418135200")] // Invalid check digit
    [InlineData("0110599520000000000100")] // Invalid check digit
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void ParseRoutingNumber_ValidCbu_ReturnsDetails()
    {
        // Arrange
        var cbu = "2850590940090418135201";

        // Act
        var result = _validator.ParseRoutingNumber(cbu);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("AR");
        result.BankCode.Should().Be("285");
        result.BranchCode.Should().Be("590");
    }

    [Fact]
    public void ParseBankAccount_ValidCbu_ReturnsDetails()
    {
        // Arrange
        var cbu = "2850590940090418135201";

        // Act
        var result = _validator.ParseBankAccount(cbu);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("AR");
        result.BranchCode.Should().Be("590");
        // CBU format: positions 8-20 (13 digits) = account number
        result.CoreAccountNumber.Should().Be("4009041813520");
    }

    [Fact]
    public void Parse_ValidCbu_ReturnsNormalizedValue()
    {
        // Arrange
        var cbu = "2850590-9-40090418135201";

        // Act
        var result = _validator.Parse(cbu);

        // Assert
        result.Should().Be("2850590940090418135201");
    }

    [Fact]
    public void Parse_InvalidCbu_ReturnsNull()
    {
        // Arrange
        var cbu = "invalid";

        // Act
        var result = _validator.Parse(cbu);

        // Assert
        result.Should().BeNull();
    }
}
