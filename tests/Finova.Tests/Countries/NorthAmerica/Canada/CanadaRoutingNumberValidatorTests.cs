using Finova.Core.Common;
using Finova.Countries.NorthAmerica.Canada.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.Canada;

public class CanadaRoutingNumberValidatorTests
{
    private readonly CanadaRoutingNumberValidator _validator = new();

    // EFT Format tests (9 digits starting with 0)
    [Theory]
    [InlineData("012345678")]
    [InlineData("000123456")]
    [InlineData("003002600")] // Example from docs
    public void Validate_ValidEftFormat_ReturnsSuccess(string input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    // MICR Format tests (8 digits, optionally with hyphen)
    [Theory]
    [InlineData("00260-003")] // MICR with hyphen
    [InlineData("00260003")] // MICR without hyphen
    [InlineData("12345-678")]
    [InlineData("12345678")]
    public void Validate_ValidMicrFormat_ReturnsSuccess(string input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("123456789")] // 9 digits but doesn't start with 0
    [InlineData("0123456")] // Too short for EFT
    [InlineData("0123456789")] // Too long
    [InlineData("01234A678")] // Non-numeric
    [InlineData("1234567")] // 7 digits
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("012345678", "123", "45678")]
    [InlineData("000100001", "001", "00001")]
    [InlineData("003002600", "030", "02600")] // EFT format
    public void ParseRoutingNumber_ValidEftInput_ReturnsDetails(string input, string expectedBank, string expectedBranch)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("CA");
        result.BankCode.Should().Be(expectedBank);
        result.BranchCode.Should().Be(expectedBranch);
    }

    [Theory]
    [InlineData("00260-003", "003", "00260")] // MICR format converted to EFT
    [InlineData("00260003", "003", "00260")]
    public void ParseRoutingNumber_ValidMicrInput_ReturnsDetails(string input, string expectedBank, string expectedBranch)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("CA");
        result.BankCode.Should().Be(expectedBank);
        result.BranchCode.Should().Be(expectedBranch);
    }

    // Test format conversions
    [Theory]
    [InlineData("00260-003", "000300260")] // MICR to EFT
    [InlineData("00260003", "000300260")]
    [InlineData("003002600", "003002600")] // EFT stays EFT (already normalized)
    public void NormalizeToEft_ConvertsCorrectly(string input, string expectedEft)
    {
        // Act
        var result = CanadaRoutingNumberValidator.NormalizeToEft(input);

        // Assert
        result.Should().Be(expectedEft);
    }

    [Theory]
    [InlineData("003002600", "02600-030")] // EFT to MICR: 0YYYXXXXX -> XXXXX-YYY
    [InlineData("000300260", "00260-003")]
    [InlineData("00260-003", "00260-003")] // MICR stays MICR (just adds hyphen)
    [InlineData("00260003", "00260-003")]
    public void ConvertToMicr_ConvertsCorrectly(string input, string expectedMicr)
    {
        // Act
        var result = CanadaRoutingNumberValidator.ConvertToMicr(input);

        // Assert
        result.Should().Be(expectedMicr);
    }
}
