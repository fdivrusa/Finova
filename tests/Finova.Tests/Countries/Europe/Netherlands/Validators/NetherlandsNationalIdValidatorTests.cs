using Finova.Countries.Europe.Netherlands.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Netherlands.Validators;

public class NetherlandsNationalIdValidatorTests
{
    [Theory]
    [InlineData("072081235")] // Valid 9 digits
    [InlineData("628848742")] // Valid 9 digits
    [InlineData("232320895")] // Valid 9 digits
    [InlineData("72081235")]  // Valid 8 digits (padded to 072081235)
    [InlineData("0720.81.235")] // Valid with dots
    public void Validate_ValidId_ReturnsSuccess(string input)
    {
        // Act
        var result = NetherlandsNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        // Act
        var result = NetherlandsNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("1234567")] // Too short
    [InlineData("1234567890")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        // Act
        var result = NetherlandsNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("07208123A")] // Invalid char
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        // Act
        var result = NetherlandsNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        // InputSanitizer removes 'A', so it becomes 8 digits "07208123".
        // "07208123" -> padded "007208123".
        // Checksum will likely fail.
        // But wait, if I pass "07208123A", sanitized is "07208123".
        // Length is 8. Valid length.
        // Padded to "007208123".
        // Checksum check.
        // So it returns InvalidChecksum, not InvalidFormat.
        // Unless I use a char that InputSanitizer keeps? No, it keeps only alphanumeric.
        // But BSN must be digits only.
        // If I pass "A72081235", sanitized "72081235" (8 digits).
        // Padded "072081235". Valid!
        // So "A72081235" is considered valid?
        // InputSanitizer removes letters.
        // If the user input contains letters, it should probably be invalid format if we expect only digits?
        // But InputSanitizer is aggressive.
        // If I want to enforce "Only Digits", I should check input BEFORE sanitization?
        // Or InputSanitizer should be used carefully.
        // Usually, we sanitize separators. But if we sanitize letters, we might accept invalid input as valid.
        // However, the requirement says "Sanitize input (trim, uppercase, remove separators)".
        // It doesn't explicitly say "remove letters".
        // But `InputSanitizer` implementation removes everything except LetterOrDigit.
        // And `NetherlandsNationalIdValidator` checks `char.IsDigit(c)` on sanitized string.
        // Since sanitized string only has LetterOrDigit, if it has letters, it fails.
        // But if `InputSanitizer` removed the letters?
        // Wait, `InputSanitizer` keeps letters!
        // `if (!char.IsLetterOrDigit(c))` -> remove.
        // So letters ARE kept.
        // So "07208123A" -> sanitized "07208123A".
        // Loop checks `IsDigit`. 'A' is not digit.
        // So it returns `InvalidFormat`.
        // Correct.
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("072081236")] // Invalid checksum (last digit changed)
    public void Validate_InvalidChecksum_ReturnsFailure(string input)
    {
        // Act
        var result = NetherlandsNationalIdValidator.ValidateStatic(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == Finova.Core.Common.ValidationErrorCode.InvalidChecksum);
    }
}
