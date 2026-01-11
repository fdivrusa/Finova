using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Serbia.Validators;

public class SerbiaBbanValidator : IBbanValidator
{
    public string CountryCode => "RS";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 3 digits (Bank) + 13 digits (Account) + 2 digits (Check)
        // Total length: 18 characters

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // Structure check: All digits
        for (int i = 0; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSerbiaIbanFormat);
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
    /// Parses the Serbian BBAN and returns the structured details.
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

        // BBAN format: 3 digits (Bank) + 13 digits (Account) + 2 digits (Check)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 3),
            AccountNumber = bban.Substring(3, 13),
            NationalCheckDigits = bban.Substring(16, 2)
        };
    }
}
