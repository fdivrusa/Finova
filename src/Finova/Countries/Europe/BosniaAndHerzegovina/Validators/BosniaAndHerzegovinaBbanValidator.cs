using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

public class BosniaAndHerzegovinaBbanValidator : IBbanValidator
{
    public string CountryCode => "BA";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        // BBAN format: 16 digits
        // Total length: 16 characters

        if (bban.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // Structure check: Digits only
        for (int i = 0; i < 16; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Bosnia and Herzegovina"));
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
    /// Parses the Bosnia and Herzegovina BBAN and returns the structured details.
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

        // BBAN format: 3 digits (Bank) + 3 digits (Branch) + 8 digits (Account) + 2 digits (Check)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 3),
            BranchCode = bban.Substring(3, 3),
            AccountNumber = bban.Substring(6, 8),
            NationalCheckDigits = bban.Substring(14, 2)
        };
    }
}
