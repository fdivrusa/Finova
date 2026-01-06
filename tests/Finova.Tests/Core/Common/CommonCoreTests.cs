using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Common;

public class CommonCoreTests
{
    #region InputSanitizer Tests

    [Theory]
    [InlineData("abc-123.DEF", "ABC123DEF")]
    [InlineData("  spaces  ", "SPACES")]
    [InlineData("12.34/56", "123456")]
    [InlineData("AlreadyClean123", "ALREADYCLEAN123")]
    public void Sanitize_RemovesNonAlphanumericAndUppercases(string input, string expected)
    {
        InputSanitizer.Sanitize(input).Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Sanitize_HandlesNullOrEmpty(string? input)
    {
        InputSanitizer.Sanitize(input).Should().Be(input);
    }

    #endregion

    #region DateHelper Tests

    [Theory]
    [InlineData(2024, 2, 29, true)] // Leap year
    [InlineData(2023, 2, 29, false)] // Not leap year
    [InlineData(2024, 1, 31, true)]
    [InlineData(2024, 13, 1, false)] // Invalid month
    [InlineData(2024, 4, 31, false)] // April has 30 days
    [InlineData(0, 1, 1, false)] // Invalid year
    [InlineData(10000, 1, 1, false)] // Invalid year
    public void IsValidDate_ValidatesCorrectly(int y, int m, int d, bool expected)
    {
        DateHelper.IsValidDate(y, m, d).Should().Be(expected);
    }

    #endregion

    #region ChecksumHelper Tests

    [Theory]
    [InlineData("79927398713", true)] // Luhn valid
    [InlineData("79927398710", false)] // Luhn invalid
    [InlineData("00000000000", true)] // All zeros
    [InlineData("abc", false)] // Non-numeric
    public void ValidateLuhn_WorksCorrectly(string input, bool expected)
    {
        ChecksumHelper.ValidateLuhn(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("7992739871", 3)] // Calculate 13th digit
    [InlineData("453201511283036", 6)] // Visa
    public void CalculateLuhnCheckDigit_WorksCorrectly(string input, int expected)
    {
        ChecksumHelper.CalculateLuhnCheckDigit(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("539007547034111468", true)] // Mod 97 (BE IBAN numeric form)
    [InlineData("539007547034111469", false)] // Invalid
    public void ValidateModulo97_WorksCorrectly(string input, bool expected)
    {
        ChecksumHelper.ValidateModulo97(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("15", true)] // Verhoeff valid
    [InlineData("12345", false)] // Verhoeff invalid
    public void ValidateVerhoeff_WorksCorrectly(string input, bool expected)
    {
        ChecksumHelper.ValidateVerhoeff(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("1234561", true)] // 7-3-1 Finnish
    [InlineData("1234560", false)]
    public void Validate731_WorksCorrectly(string input, bool expected)
    {
        ChecksumHelper.Validate731(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("210000000003139471430009017", true)] // Mod10 Recursive Swiss
    [InlineData("210000000003139471430009010", false)]
    public void ValidateMod10Recursive_WorksCorrectly(string input, bool expected)
    {
        ChecksumHelper.ValidateMod10Recursive(input).Should().Be(expected);
    }

    #endregion

    #region ValidationResult Tests

    [Fact]
    public void ValidationResult_Success_HasNoErrors()
    {
        var result = ValidationResult.Success();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidationResult_Failure_HasErrors()
    {
        var result = ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Message");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidChecksum);
        result.Errors[0].Message.Should().Be("Message");
    }

    #endregion
}
