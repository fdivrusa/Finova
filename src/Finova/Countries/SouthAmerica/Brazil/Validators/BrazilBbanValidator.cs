using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validator for Brazil BBAN.
/// Format: 25 characters (8 digits bank code + 5 digits branch code + 10 digits account number + 1 letter account type + 1 letter owner indicator).
/// </summary>
public class BrazilBbanValidator : IBbanValidator
{
    private const int BbanLength = 25;

    public string CountryCode => "BR";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Brazil BBAN.
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

        // First 8 characters must be digits (bank code)
        for (int i = 0; i < 8; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeDigits);
            }
        }

        // Next 5 characters must be digits (branch code)
        for (int i = 8; i < 13; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BranchCodeMustBeDigits);
            }
        }

        // Next 10 characters must be digits (account number)
        for (int i = 13; i < 23; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeDigits);
            }
        }

        // Position 24 (index 23): Account type - must be a letter
        if (!char.IsLetter(bban[23]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidAccountType);
        }

        // Position 25 (index 24): Owner indicator - must be alphanumeric
        if (!char.IsLetterOrDigit(bban[24]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidOwnerIndicator);
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

        // Brazil BBAN: Bank(8 digits) + Branch(5 digits) + Account(10 digits) + Type(1 letter) + Owner(1 letter)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..8],
            BranchCode = sanitized.Substring(8, 5),
            AccountNumber = sanitized.Substring(13, 10),
            NationalCheckDigits = sanitized[23..] // Account type + Owner indicator
        };
    }
}
