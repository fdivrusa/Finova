using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Benin.Validators;

/// <summary>
/// Validator for Benin BBAN.
/// Format: 24 characters (1 letter + 23 digits).
/// </summary>
public class BeninBbanValidator : IBbanValidator
{
    public string CountryCode => "BJ";
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

        // UEMOA standard: 1 letter + 23 digits
        if (!char.IsLetter(bban[0]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BbanMustStartWithLetter);
        }

        for (int i = 1; i < 24; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BbanMustContainDigitsAfterLetter);
            }
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;

    /// <inheritdoc/>
    public BbanDetails? ParseDetails(string? bban)
    {
        var sanitized = InputSanitizer.Sanitize(bban);
        if (!Validate(sanitized).IsValid)
        {
            return null;
        }

        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..5],
            BranchCode = sanitized.Substring(5, 5),
            AccountNumber = sanitized.Substring(10, 12),
            NationalCheckDigits = sanitized.Substring(22, 2)
        };
    }
}
