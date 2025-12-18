using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish BBAN (Basic Bank Account Number).
/// Format: Entidad (4) + Oficina (4) + DC (2) + Cuenta (10).
/// </summary>
public class SpainBbanValidator : IBbanValidator
{
    private const int SpainBbanLength = 20;

    public string CountryCode => "ES";

    // Standard weights for Spanish Modulo 11 algorithm
    private static readonly int[] Weights = [1, 2, 4, 8, 5, 10, 9, 7, 3, 6];

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Spanish BBAN.
    /// </summary>
    /// <param name="bban">The BBAN string to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != SpainBbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, SpainBbanLength, bban.Length));
        }

        for (int i = 0; i < SpainBbanLength; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Spain"));
            }
        }

        // Extract parts:
        // Bank (Entidad): 4 chars (Pos 0-4)
        // Branch (Oficina): 4 chars (Pos 4-8)
        // DC (Check Digits): 2 chars (Pos 8-10)
        // Account (Cuenta): 10 chars (Pos 10-20)

        string bank = bban.Substring(0, 4);
        string branch = bban.Substring(4, 4);
        string dc = bban.Substring(8, 2);
        string account = bban.Substring(10, 10);

        // Calculate First DC (Validates Bank + Branch)
        // Input must be padded with "00" to make 10 digits: "00" + Bank + Branch
        int calculatedDc1 = CalculateSpanishDigit("00" + bank + branch);

        // Calculate Second DC (Validates Account)
        // Input is just the 10-digit Account
        int calculatedDc2 = CalculateSpanishDigit(account);

        // Verify against the DC present in the BBAN
        // dc[0] is the first digit, dc[1] is the second
        if ((dc[0] - '0') != calculatedDc1 || (dc[1] - '0') != calculatedDc2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSpanishDcCheck);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Calculates a single Spanish Control Digit (Modulo 11).
    /// </summary>
    /// <param name="value">A 10-digit numeric string.</param>
    /// <returns>The calculated check digit (0-9).</returns>
    private static int CalculateSpanishDigit(string value)
    {
        if (value.Length != 10)
        {
            return -1; // Should not happen given logic above
        }

        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            // Character '0' has int value 48. subtracting '0' gives the numeric value.
            int digit = value[i] - '0';
            sum += digit * Weights[i];
        }

        int remainder = 11 - (sum % 11);
        if (remainder == 11) return 0;
        if (remainder == 10) return 1;
        return remainder;
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string sanitized = input.Replace(" ", "").Replace("-", "").Trim();
        return Validate(sanitized).IsValid ? sanitized : null;
    }
}
