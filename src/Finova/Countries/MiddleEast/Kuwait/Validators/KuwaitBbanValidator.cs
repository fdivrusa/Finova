using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.MiddleEast.Kuwait.Validators;

public class KuwaitBbanValidator : IBbanValidator
{
    public string CountryCode => "KW";

    public ValidationResult Validate(string? bban)
    {
        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 4a 22c -> 26 chars
        if (bban.Length != 26)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "KW BBAN must be 26 characters.");
        }

        if (!bban.All(char.IsLetterOrDigit))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "KW BBAN must be alphanumeric.");
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? bban) => Validate(bban).IsValid ? bban?.Replace(" ", "").Replace("-", "").Trim() : null;

    public BbanDetails? ParseDetails(string? bban)
    {
        if (Validate(bban).IsValid)
        {
            // Bank 4, Account 22
            return new BbanDetails
            {
                Bban = bban!,
                CountryCode = CountryCode,
                BankCode = bban![..4],
                BranchCode = "",
                AccountNumber = bban[4..]
            };
        }
        return null;
    }
}
