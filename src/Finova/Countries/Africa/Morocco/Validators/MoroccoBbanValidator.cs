using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Morocco.Validators;

/// <summary>
/// Validator for Morocco BBAN.
/// Format: 24 digits.
/// </summary>
public class MoroccoBbanValidator : IBbanValidator
{
    public string CountryCode => "MA";
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

        // Morocco: Bank(3) + Branch(4) + Account(15) + Key(2)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..3],
            BranchCode = sanitized.Substring(3, 4),
            AccountNumber = sanitized.Substring(7, 15),
            NationalCheckDigits = sanitized.Substring(22, 2)
        };
    }
}
