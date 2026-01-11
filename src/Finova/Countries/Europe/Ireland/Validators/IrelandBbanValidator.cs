using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ireland.Validators;

public class IrelandBbanValidator : IBbanValidator
{
    public string CountryCode => "IE";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 4 letters (Bank) + 6 digits (Sort Code) + 8 digits (Account)
        // Total length: 18 characters

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // 1. Bank Code (Pos 0-4): 4 letters
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.IrelandBankCodeMustBeLetters);
            }
        }

        // 2. Sort Code (Pos 4-10): 6 digits
        for (int i = 4; i < 10; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.IrelandSortCodeAccountNumberMustBeDigits);
            }
        }

        // 3. Account Number (Pos 10-18): 8 digits
        for (int i = 10; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.IrelandSortCodeAccountNumberMustBeDigits);
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Parses the Irish BBAN and returns the structured details.
    /// </summary>
    /// <param name="bban">The BBAN string to parse.</param>
    /// <returns>A BbanDetails object if valid; otherwise, null.</returns>
    public BbanDetails? ParseDetails(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (!Validate(bban).IsValid)
        {
            return null;
        }

        // BBAN format: 4 letters (Bank) + 6 digits (Sort Code) + 8 digits (Account)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 4),
            BranchCode = bban.Substring(4, 6),
            AccountNumber = bban.Substring(10, 8)
        };
    }
}
