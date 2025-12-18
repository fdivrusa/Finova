using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norwegian BBAN (Basic Bank Account Number).
/// </summary>
public class NorwayBbanValidator : IBbanValidator
{
    public string CountryCode => "NO";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Norwegian BBAN structure and checksum.
    /// Format: 4 Bank + 6 Account + 1 Check (Total 11 digits).
    /// Algorithm: Modulo 11 with weights.
    /// </summary>
    /// <param name="bban">The BBAN string (11 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 11, bban.Length));
        }

        // Ensure all characters are digits
        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNorwayIbanFormat);
            }
        }

        // Internal Validation: Modulo 11 on BBAN
        if (!ValidateMod11(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNorwayBbanChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates Norwegian BBAN using Modulo 11 with weights.
    /// Weights sequence: 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 on the first 10 digits.
    /// </summary>
    private static bool ValidateMod11(string bban)
    {
        // BBAN is 11 digits. Last one is check digit.
        ReadOnlySpan<int> weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += (bban[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else if (remainder == 1)
        {
            return false; // Mod11 "10" exception (invalid account)
        }
        else
        {
            checkDigit = 11 - remainder;
        }

        return checkDigit == (bban[10] - '0');
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
