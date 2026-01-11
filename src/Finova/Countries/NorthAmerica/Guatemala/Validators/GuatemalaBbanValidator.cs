using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Guatemala.Validators;

/// <summary>
/// Validator for Guatemala BBAN.
/// Format: 24 characters (4 letters bank code + 20 alphanumeric account number).
/// </summary>
public class GuatemalaBbanValidator : IBbanValidator
{
    private const int BbanLength = 24;

    public string CountryCode => "GT";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Guatemala BBAN.
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

        // Remaining 20 characters must be alphanumeric (account number)
        for (int i = 4; i < BbanLength; i++)
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

        // Guatemala BBAN: Bank(4 letters) + Account(20 alphanumeric)
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
