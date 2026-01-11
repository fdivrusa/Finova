using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Russia.Validators;

/// <summary>
/// Validator for Russia BBAN.
/// Format: 29 digits.
/// </summary>
public class RussiaBbanValidator : IBbanValidator
{
    public string CountryCode => "RU";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 29)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 29, bban.Length));
        }

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
            }
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;

    /// <summary>
    /// Parses the Russian BBAN and returns the structured details.
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

        // BBAN format: 9 digits (Bank/BIC) + 20 digits (Account)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 9),
            AccountNumber = bban.Substring(9, 20)
        };
    }
}
