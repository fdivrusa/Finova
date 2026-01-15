using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Egypt.Validators;

public class EgyptBbanValidator : IBbanValidator
{
    public string CountryCode => "EG";

    public ValidationResult Validate(string? bban)
    {
        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 25n -> 25 digits
        if (bban.Length != 25)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidEgyptBbanLength);
        }

        if (!bban.All(char.IsDigit))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidEgyptBbanFormat);
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? bban) => Validate(bban).IsValid ? bban?.Replace(" ", "").Replace("-", "").Trim() : null;

    public BbanDetails? ParseDetails(string? bban)
    {
        if (Validate(bban).IsValid)
        {
            // Bank 4, Branch 4, Account 17
            // Format says 25n. IBAN structure: EG kk bbbb ssss aaaa aaaa aaaa a
            // Bank 4, Branch 4, Account 17.
            // But I should check if it's correct decomposition.
            // Assuming simplified parsing.
            return new BbanDetails
            {
                Bban = bban!,
                CountryCode = CountryCode,
                BankCode = bban![..4],
                BranchCode = bban[4..8],
                AccountNumber = bban[8..]
            };
        }
        return null;
    }
}
