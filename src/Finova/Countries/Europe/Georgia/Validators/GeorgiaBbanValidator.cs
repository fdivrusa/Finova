using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Georgia.Validators;

/// <summary>
/// Validator for Georgia BBAN.
/// </summary>
public class GeorgiaBbanValidator : IBbanValidator
{
    public string CountryCode => "GE";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Georgia BBAN.
    /// </summary>
    /// <param name="bban">The BBAN to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 18, bban.Length));
        }

        // Bank Code (2 chars)
        for (int i = 0; i < 2; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeAlphanumeric);
            }
        }

        // Account Number (16 chars)
        for (int i = 2; i < 18; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeAlphanumeric);
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
    /// Parses the Georgian BBAN and returns the structured details.
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

        // BBAN format: 2 alphanumeric (Bank) + 16 alphanumeric (Account)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 2),
            AccountNumber = bban.Substring(2, 16)
        };
    }
}
