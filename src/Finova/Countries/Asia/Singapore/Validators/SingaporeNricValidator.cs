using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Singapore.Validators;

/// <summary>
/// Validates Singapore NRIC (National Registration Identity Card) and FIN (Foreign Identification Number).
/// </summary>
public class SingaporeNricValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "SG";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates a Singapore NRIC or FIN.
    /// </summary>
    /// <param name="nric">The NRIC/FIN string (e.g., "S1234567A").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nric)
    {
        if (string.IsNullOrWhiteSpace(nric))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = InputSanitizer.Sanitize(nric);
        if (string.IsNullOrEmpty(clean))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidNricLength);
        }

        char firstChar = clean[0];
        if (!"STFGM".Contains(firstChar))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNricPrefix);
        }

        if (!clean.Substring(1, 7).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNricDigits);
        }

        int[] weights = { 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            sum += (clean[i + 1] - '0') * weights[i];
        }

        if (firstChar == 'T' || firstChar == 'G')
        {
            sum += 4;
        }
        else if (firstChar == 'M')
        {
            sum += 3;
        }

        int remainder = sum % 11;
        char checkDigit = clean[8];
        char expectedCheckDigit;

        if (firstChar == 'S' || firstChar == 'T')
        {
            // S/T: J, Z, I, H, G, F, E, D, C, B, A
            char[] stMap = { 'J', 'Z', 'I', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A' };
            expectedCheckDigit = stMap[remainder];
        }
        else if (firstChar == 'F' || firstChar == 'G')
        {
            // F/G: X, W, U, T, R, Q, P, N, M, L, K
            char[] fgMap = { 'X', 'W', 'U', 'T', 'R', 'Q', 'P', 'N', 'M', 'L', 'K' };
            expectedCheckDigit = fgMap[remainder];
        }
        else // M
        {
            // M: X, W, U, T, R, Q, P, N, J, L, K
            char[] mMap = { 'X', 'W', 'U', 'T', 'R', 'Q', 'P', 'N', 'J', 'L', 'K' };
            expectedCheckDigit = mMap[remainder];
        }

        if (checkDigit != expectedCheckDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNricChecksum);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
