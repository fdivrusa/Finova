using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.India.Validators;

/// <summary>
/// Validates Indian Aadhaar numbers.
/// </summary>
public class IndiaAadhaarValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "IN";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace(" ", "").Replace("-", "");
        }
        return null;
    }

    /// <summary>
    /// Validates an Indian Aadhaar number (Static).
    /// </summary>
    /// <param name="aadhaar">The Aadhaar number (12 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? aadhaar)
    {
        return ValidateAadhaar(aadhaar);
    }

    /// <summary>
    /// Validates an Indian Aadhaar number.
    /// </summary>
    /// <param name="aadhaar">The Aadhaar number (12 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateAadhaar(string? aadhaar)
    {
        if (string.IsNullOrWhiteSpace(aadhaar))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = aadhaar.Replace(" ", "").Replace("-", "");

        if (clean.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidIndiaAadhaarLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIndiaAadhaarFormat);
        }

        // Verhoeff Algorithm Validation
        if (!ChecksumHelper.ValidateVerhoeff(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidIndiaAadhaarChecksum);
        }

        return ValidationResult.Success();
    }
}
