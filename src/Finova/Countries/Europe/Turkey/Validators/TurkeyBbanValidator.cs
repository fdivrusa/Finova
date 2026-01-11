using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Turkey.Validators;

public class TurkeyBbanValidator : IBbanValidator
{
    public string CountryCode => "TR";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 5 digits (Bank) + 1 alphanumeric (Reserve) + 16 alphanumeric (Account)
        // Total length: 22 characters

        if (bban.Length != 22)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // 1. Bank Code (Pos 0-5): 5 digits
        for (int i = 0; i < 5; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTurkeyBankCode);
            }
        }

        // 2. Reserve (Pos 5): 1 alphanumeric character
        if (!char.IsLetterOrDigit(bban[5]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTurkeyReserveChar);
        }

        // 3. Account Number (Pos 6-22): 16 alphanumeric characters
        for (int i = 6; i < 22; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTurkeyAccountNumber);
            }
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }

    /// <summary>
    /// Parses the Turkish BBAN and returns the structured details.
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

        // BBAN format: 5 digits (Bank) + 1 alphanumeric (Reserve) + 16 alphanumeric (Account)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 5),
            AccountNumber = bban.Substring(6, 16),
            NationalCheckDigits = bban.Substring(5, 1)
        };
    }
}
