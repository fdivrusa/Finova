using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;

/// <summary>
/// Validator for British Virgin Islands BBAN.
/// Format: 20 characters (4 letters bank code + 16 digits account number).
/// </summary>
public class VirginIslandsBritishBbanValidator : IBbanValidator
{
    private const int BbanLength = 20;

    public string CountryCode => "VG";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the British Virgin Islands BBAN.
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

        if (bban.Length != BbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, BbanLength, bban.Length));
        }

        // First 4 characters must be letters (bank code)
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeLetters);
            }
        }

        // Remaining 16 characters must be digits (account number)
        for (int i = 4; i < BbanLength; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeDigits);
            }
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        string sanitized = InputSanitizer.Sanitize(input)!.ToUpperInvariant();
        return Validate(sanitized).IsValid ? sanitized : null;
    }

    /// <inheritdoc/>
    public BbanDetails? ParseDetails(string? bban)
    {
        var sanitized = InputSanitizer.Sanitize(bban)?.ToUpperInvariant();
        if (!Validate(sanitized).IsValid)
        {
            return null;
        }

        // British Virgin Islands BBAN: Bank(4 letters) + Account(16 digits)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..4],
            BranchCode = null,
            AccountNumber = sanitized[4..],
            NationalCheckDigits = null
        };
    }
}
