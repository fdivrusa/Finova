using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Denmark.Validators;

public class DenmarkBbanValidator : IBbanValidator
{
    public string CountryCode => "DK";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 14 digits
        // Total length: 14 characters

        if (bban.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // Structure check: Digits only
        for (int i = 0; i < 14; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.DenmarkIbanMustContainOnlyDigits);
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
    /// Parses the Danish BBAN and returns the structured details.
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

        // BBAN format: 4 digits (Bank) + 10 digits (Account, includes check digit at end)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 4),
            AccountNumber = bban.Substring(4, 10)
        };
    }
}
