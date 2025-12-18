using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Cyprus.Validators;

public class CyprusBbanValidator : IBbanValidator
{
    public string CountryCode => "CY";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        // BBAN format: 3 digits (Bank) + 5 digits (Branch) + 16 alphanumeric (Account)
        // Total length: 24 characters

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-3): Must be digits
        for (int i = 0; i < 3; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CyprusIbanInvalidBankCode);
            }
        }

        // 2. Branch Code (Pos 3-8): Must be digits
        for (int i = 3; i < 8; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CyprusIbanInvalidBranchCode);
            }
        }

        // 3. Account Number (Pos 8-24): Alphanumeric
        for (int i = 8; i < 24; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CyprusIbanInvalidAccount);
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
