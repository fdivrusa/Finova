using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Mauritania.Validators;

public class MauritaniaBbanValidator : IBbanValidator
{
    public string CountryCode => "MR";

    public ValidationResult Validate(string? bban)
    {
        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 23n -> 23 digits
        if (bban.Length != 23)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "MR BBAN must be 23 digits.");
        }

        if (!bban.All(char.IsDigit))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "MR BBAN must contain only digits.");
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? bban) => Validate(bban).IsValid ? bban?.Replace(" ", "").Replace("-", "").Trim() : null;

    public BbanDetails? ParseDetails(string? bban)
    {
        if (Validate(bban).IsValid)
        {
            // Bank 5, Branch 5, Account 11, Key 2
            return new BbanDetails
            {
                Bban = bban!,
                CountryCode = CountryCode,
                BankCode = bban![..5],
                BranchCode = bban[5..10],
                AccountNumber = bban[10..21]
            };
        }
        return null;
    }
}
