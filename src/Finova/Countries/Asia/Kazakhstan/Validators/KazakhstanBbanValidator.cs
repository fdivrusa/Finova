using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Kazakhstan.Validators;

/// <summary>
/// Validator for Kazakhstan BBAN.
/// Format: 16 characters (3 digits bank code + 13 alphanumeric account number).
/// </summary>
public class KazakhstanBbanValidator : IBbanValidator
{
    private const int BbanLength = 16;

    public string CountryCode => "KZ";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Kazakhstan BBAN.
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

        // First 3 characters must be digits (bank code)
        for (int i = 0; i < 3; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeDigits);
            }
        }

        // Remaining 13 characters must be alphanumeric (account number)
        for (int i = 3; i < BbanLength; i++)
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
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        string sanitized = InputSanitizer.Sanitize(input)!;
        return Validate(sanitized).IsValid ? sanitized : null;
    }

    /// <inheritdoc/>
    public BbanDetails? ParseDetails(string? bban)
    {
        var sanitized = InputSanitizer.Sanitize(bban);
        if (!Validate(sanitized).IsValid)
        {
            return null;
        }

        // Kazakhstan BBAN: Bank(3 digits) + Account(13 alphanumeric)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..3],
            BranchCode = null,
            AccountNumber = sanitized[3..],
            NationalCheckDigits = null
        };
    }
}
