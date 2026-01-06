using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Barbados.Validators;

/// <summary>
/// Validator for Barbados BBAN.
/// Format: 4 letters + 20 digits.
/// </summary>
public class BarbadosBbanValidator : IBbanValidator
{
    public string CountryCode => "BB";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 24, bban.Length));
        }

        // First 4 must be letters
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "First 4 characters must be letters.");
            }
        }

        // Remaining 20 must be digits
        for (int i = 4; i < 24; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Last 20 characters must be digits.");
            }
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;
}
