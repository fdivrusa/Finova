using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Greece.Validators;

/// <summary>
/// Validator for Greece BBAN.
/// </summary>
public class GreeceBbanValidator : IBbanValidator
{
    public string CountryCode => "GR";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Greece BBAN.
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

        if (bban.Length != 23)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 23, bban.Length));
        }

        // Bank Code (3 digits) + Branch Code (4 digits)
        // Indices 0-6 must be digits
        for (int i = 0; i < 7; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.GreeceBankBranchCodeMustBeDigits);
            }
        }

        // Account Number (16 alphanumeric)
        // Indices 7-22
        for (int i = 7; i < 23; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.GreeceAccountNumberMustBeAlphanumeric);
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
    /// Parses the Greek BBAN and returns the structured details.
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

        // BBAN format: 3 digits (Bank) + 4 digits (Branch) + 16 alphanumeric (Account)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 3),
            BranchCode = bban.Substring(3, 4),
            AccountNumber = bban.Substring(7, 16)
        };
    }
}
