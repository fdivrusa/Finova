using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Latvia.Validators;

/// <summary>
/// Validator for Latvia BBAN.
/// </summary>
public class LatviaBbanValidator : IBbanValidator
{
    public string CountryCode => "LV";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Latvia BBAN.
    /// </summary>
    /// <param name="bban">The BBAN to validate.</param>
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

        // Bank Code (4 chars) + Account (13 chars)
        // All alphanumeric
        foreach (char c in bban)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.LatviaIbanMustBeAlphanumeric);
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
