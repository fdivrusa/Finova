using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Slovenia.Validators;

public class SloveniaBbanValidator : IBbanValidator
{
    public string CountryCode => "SI";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 5 digits (Bank) + 8 digits (Account) + 2 digits (Check)
        // Total length: 15 characters

        if (bban.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // Structure check: Digits only
        for (int i = 0; i < 15; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Slovenia"));
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
    /// Parses the Slovenian BBAN and returns the structured details.
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

        // BBAN format: 5 digits (Bank) + 8 digits (Account) + 2 digits (Check)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 5),
            AccountNumber = bban.Substring(5, 8),
            NationalCheckDigits = bban.Substring(13, 2)
        };
    }
}
