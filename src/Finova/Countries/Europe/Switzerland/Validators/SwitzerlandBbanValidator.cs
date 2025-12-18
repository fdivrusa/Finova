using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Switzerland.Validators;

/// <summary>
/// Validator for Switzerland BBAN (Basic Bank Account Number).
/// </summary>
public class SwitzerlandBbanValidator : IBbanValidator
{
    public string CountryCode => "CH";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Switzerland BBAN structure.
    /// Format: 5 digits (Clearing) + 12 alphanumeric (Account).
    /// </summary>
    /// <param name="bban">The BBAN string (17 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 17)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 17, bban.Length));
        }

        // 1. Clearing Number (Pos 0-5): 5 digits
        for (int i = 0; i < 5; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSwitzerlandClearingNumberFormat);
            }
        }

        // 2. Account Number (Pos 5-17): 12 alphanumeric
        for (int i = 5; i < 17; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSwitzerlandAccountNumberFormat);
            }
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
