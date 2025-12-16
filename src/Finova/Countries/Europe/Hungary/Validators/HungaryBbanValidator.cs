using Finova.Core.Common;

namespace Finova.Countries.Europe.Hungary.Validators;

/// <summary>
/// Validator for Hungary BBAN.
/// </summary>
public static class HungaryBbanValidator
{
    /// <summary>
    /// Validates the Hungary BBAN.
    /// </summary>
    /// <param name="bban">The BBAN to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string bban)
    {
        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 24, bban.Length));
        }

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.HungaryIbanMustBeDigits);
            }
        }

        return ValidationResult.Success();
    }
}
