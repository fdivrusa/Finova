using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Seychelles.Validators;

/// <summary>
/// Validator for Seychelles BBAN.
/// Format: 27 characters (4 letters Bank, 20 digits Account, 3 letters Currency).
/// </summary>
public class SeychellesBbanValidator : IBbanValidator
{
    public string CountryCode => "SC";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 27)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 27, bban.Length));
        }

        // Format: 4 letters (Bank), 20 digits (Account), 3 letters (Currency)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bank code must be letters.");
        }

        if (!bban.Substring(4, 20).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Account number must be digits.");
        }

        if (!bban.Substring(24, 3).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Currency code must be letters.");
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

        // Seychelles: Bank(4 letters) + Branch(4 digits) + Account(16 digits) + Currency(3 letters)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..4],
            BranchCode = sanitized.Substring(4, 4),
            AccountNumber = sanitized.Substring(8, 16),
            NationalCheckDigits = sanitized.Substring(24, 3) // Currency code
        };
    }
}
