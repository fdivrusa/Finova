using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.MiddleEast.Qatar.Validators;

public class QatarBbanValidator : IBbanValidator
{
    public string CountryCode => "QA";

    public ValidationResult Validate(string? bban)
    {
        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 4a 21c -> 25 chars
        if (bban.Length != 25)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "QA BBAN must be 25 characters.");
        }

        if (!bban.All(char.IsLetterOrDigit))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "QA BBAN must be alphanumeric.");
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? bban) => Validate(bban).IsValid ? bban?.Replace(" ", "").Replace("-", "").Trim() : null;

    public BbanDetails? ParseDetails(string? bban)
    {
        if (Validate(bban).IsValid)
        {
            // Bank 4, Account 21
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
