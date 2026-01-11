using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Algeria.Validators;

/// <summary>
/// Validator for Algeria BBAN.
/// Format: 20 digits.
/// </summary>
public class AlgeriaBbanValidator : IBbanValidator
{
    public string CountryCode => "DZ";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 20, bban.Length));
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

        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..3],
            BranchCode = sanitized.Substring(3, 5),
            AccountNumber = sanitized.Substring(8, 10),
            NationalCheckDigits = sanitized.Substring(18, 2)
        };
    }
}
