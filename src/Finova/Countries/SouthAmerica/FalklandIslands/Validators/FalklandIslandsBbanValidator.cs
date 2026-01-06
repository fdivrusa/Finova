using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.FalklandIslands.Validators;

/// <summary>
/// Validator for Falkland Islands BBAN.
/// Format: 2 letters + 12 digits.
/// </summary>
public class FalklandIslandsBbanValidator : IBbanValidator
{
    public string CountryCode => "FK";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 14, bban.Length));
        }

        // First 2 must be letters
        for (int i = 0; i < 2; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "First 2 characters must be letters.");
            }
        }

        // Remaining 12 must be digits
        for (int i = 2; i < 14; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Last 12 characters must be digits.");
            }
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;
}
