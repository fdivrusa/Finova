using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for Germany BBAN.
/// </summary>
public class GermanyBbanValidator : IBbanValidator
{
    public string CountryCode => "DE";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Germany BBAN.
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

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.GermanyIbanMustContainOnlyDigits);
            }
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string sanitized = input.Replace(" ", "").Replace("-", "").Trim();
        return Validate(sanitized).IsValid ? sanitized : null;
    }

    /// <inheritdoc/>
    public BbanDetails? ParseDetails(string? bban)
    {
        if (string.IsNullOrWhiteSpace(bban)) return null;
        
        string sanitized = bban.Replace(" ", "").Replace("-", "").Trim();
        
        if (!Validate(sanitized).IsValid) return null;

        // German BBAN: 8-digit bank code (BLZ) + 10-digit account number
        return new BbanDetails
        {
            Bban = sanitized,
            CountryCode = CountryCode,
            BankCode = sanitized[..8],           // Bankleitzahl (8 digits)
            BranchCode = null,                    // Germany doesn't have separate branch codes
            AccountNumber = sanitized[8..],       // Kontonummer (10 digits)
            NationalCheckDigits = null            // Check digit is embedded in account number
        };
    }
}
