using Finova.Core.Common;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for Germany BBAN.
/// </summary>
public static class GermanyBbanValidator
{
    /// <summary>
    /// Validates the Germany BBAN.
    /// </summary>
    /// <param name="bban">The BBAN to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string bban)
    {
        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 18, bban.Length));
        }

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.GermanyIbanMustContainOnlyDigits);
            }
        }

        return ValidationResult.Success();
    }
}
