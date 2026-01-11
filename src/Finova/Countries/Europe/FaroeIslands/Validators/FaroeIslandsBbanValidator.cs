using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.FaroeIslands.Validators;

/// <summary>
/// Validator for Faroe Islands BBAN.
/// </summary>
public class FaroeIslandsBbanValidator : IBbanValidator
{
    public string CountryCode => "FO";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Faroe Islands BBAN.
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
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Faroe Islands"));
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
    /// Parses the Faroe Islands BBAN and returns the structured details.
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

        // BBAN format: 4 digits (Bank) + 9 digits (Account) + 1 digit (Check)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 4),
            AccountNumber = bban.Substring(4, 9),
            NationalCheckDigits = bban.Substring(13, 1)
        };
    }
}
