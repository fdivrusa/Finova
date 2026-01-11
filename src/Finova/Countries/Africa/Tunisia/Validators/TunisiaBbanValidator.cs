using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Tunisia.Validators;

/// <summary>
/// Validator for Tunisia BBAN.
/// Format: 20 digits.
/// </summary>
public class TunisiaBbanValidator : IBbanValidator
{
    public string CountryCode => "TN";
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

        // Tunisia: Bank(2) + Branch(3) + Account(13) + Key(2)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..2],
            BranchCode = sanitized.Substring(2, 3),
            AccountNumber = sanitized.Substring(5, 13),
            NationalCheckDigits = sanitized.Substring(18, 2)
        };
    }
}
