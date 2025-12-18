using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Portugal.Validators;

/// <summary>
/// Validator for Portuguese BBAN (NIB - Número de Identificação Bancária).
/// </summary>
public class PortugalBbanValidator : IBbanValidator
{
    public string CountryCode => "PT";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Portuguese BBAN (NIB) structure and checksum.
    /// Format: 4 Bank + 4 Branch + 11 Account + 2 Check (Total 21 digits).
    /// Algorithm: 98 - (Body % 97) == Check.
    /// </summary>
    /// <param name="bban">The BBAN string (21 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 21)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 21, bban.Length));
        }

        // Ensure all characters are digits
        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Portugal"));
            }
        }

        // Extract the parts relevant to NIB
        // Bank (4) + Branch (4) + Account (11) = 19 digits Body
        // Check (2) = last 2 digits
        ReadOnlySpan<char> bbanSpan = bban.AsSpan();
        ReadOnlySpan<char> nibBody = bbanSpan.Slice(0, 19);
        ReadOnlySpan<char> nibKey = bbanSpan.Slice(19, 2);

        if (!decimal.TryParse(nibBody, out decimal bodyValue))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Portugal"));
        }

        // Calculate remainder
        var remainder = (int)(bodyValue % 97);

        // Calculate check digit
        var checkValue = 98 - remainder;

        // Format to 2 digits (e.g., 7 becomes "07")
        // We can compare integers directly instead of strings
        if (!int.TryParse(nibKey, out int expectedCheckValue))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Portugal"));
        }

        return checkValue == expectedCheckValue
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNibKey);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
