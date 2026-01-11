using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.TimorLeste.Validators;

/// <summary>
/// Validator for Timor-Leste (East Timor) BBAN.
/// Format: 19 digits (3 bank code + 14 account number + 2 check digits).
/// </summary>
public class TimorLesteBbanValidator : IBbanValidator
{
    private const int BbanLength = 19;

    public string CountryCode => "TL";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Timor-Leste BBAN.
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

        // All 19 characters must be digits
        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
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

        // Timor-Leste BBAN: Bank(3 digits) + Account(14 digits) + Check(2 digits)
        return new BbanDetails
        {
            Bban = sanitized!,
            CountryCode = CountryCode,
            BankCode = sanitized![..3],
            BranchCode = null,
            AccountNumber = sanitized.Substring(3, 14),
            NationalCheckDigits = sanitized.Substring(17, 2)
        };
    }
}
