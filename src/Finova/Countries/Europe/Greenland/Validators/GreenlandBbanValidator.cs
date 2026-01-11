using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Greenland.Validators;

/// <summary>
/// Validator for Greenland BBAN.
/// </summary>
public class GreenlandBbanValidator : IBbanValidator
{
    public string CountryCode => "GL";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Greenland BBAN.
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

        if (bban.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 14, bban.Length));
        }

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.GreenlandIbanMustContainOnlyDigits);
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
    /// Parses the Greenland BBAN and returns the structured details.
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
