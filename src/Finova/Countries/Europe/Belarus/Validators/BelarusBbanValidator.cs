using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Belarus.Validators;

public class BelarusBbanValidator : IBbanValidator
{
    public string CountryCode => "BY";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }

    /// <inheritdoc/>
    public BbanDetails? ParseDetails(string? bban)
    {
        var sanitized = InputSanitizer.Sanitize(bban);
        if (string.IsNullOrWhiteSpace(sanitized))
        {
            return null;
        }

        var result = Validate(sanitized);
        if (!result.IsValid)
        {
            return null;
        }

        return new BbanDetails
        {
            Bban = sanitized,
            CountryCode = CountryCode,
            BankCode = sanitized[..4],
            BranchCode = sanitized.Substring(4, 4), // Balance Account
            AccountNumber = sanitized.Substring(8)
        };
    }

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        // BBAN format: 4 alphanumeric (Bank) + 20 alphanumeric (Account)
        // Total length: 24 characters

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // 1. Bank Code (Pos 0-4): 4 alphanumeric
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatAlphanumeric, "Belarus"));
            }
        }

        // 2. Balance Account (Pos 4-8): 4 digits
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatDigits, "Belarus"));
            }
        }

        // 3. Account Number (Pos 8-24): 16 alphanumeric
        for (int i = 8; i < 24; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatAlphanumeric, "Belarus"));
            }
        }

        return ValidationResult.Success();
    }
}
