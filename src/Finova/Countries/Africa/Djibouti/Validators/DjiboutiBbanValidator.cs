using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Djibouti.Validators;

/// <summary>
/// Validator for Djibouti BBAN.
/// Format: 23 digits.
/// </summary>
public class DjiboutiBbanValidator : IBbanValidator
{
    public string CountryCode => "DJ";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 23)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 23, bban.Length));
        }

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
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

        // Djibouti uses French-style structure: Bank(5) + Branch(5) + Account(11) + Key(2)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..5],
            BranchCode = sanitized.Substring(5, 5),
            AccountNumber = sanitized.Substring(10, 11),
            NationalCheckDigits = sanitized.Substring(21, 2)
        };
    }
}
