using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Slovakia.Validators;

/// <summary>
/// Validator for Slovak BBAN (Basic Bank Account Number).
/// </summary>
public class SlovakiaBbanValidator : IBbanValidator
{
    public string CountryCode => "SK";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Slovak BBAN structure and checksum.
    /// Format: 4 Bank + 6 Prefix + 10 Account (Total 20 digits).
    /// Algorithm: Weighted Modulo 11 for Prefix and Account.
    /// </summary>
    /// <param name="bban">The BBAN string (20 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 20, bban.Length));
        }

        // Ensure all characters are digits
        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Slovakia"));
            }
        }

        // Extract parts
        // Bank (4) - No checksum
        // Prefix (6) - Mod 11
        // Account (10) - Mod 11
        ReadOnlySpan<char> bbanSpan = bban.AsSpan();
        ReadOnlySpan<char> prefix = bbanSpan.Slice(4, 6);
        ReadOnlySpan<char> accountNumber = bbanSpan.Slice(10, 10);

        // Validate Prefix (only if not all zeros)
        if (long.TryParse(prefix, out long prefixVal) && prefixVal > 0)
        {
            if (!ValidateSlovakMod11(prefix, true))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakPrefixChecksum);
            }
        }

        // Validate Account Number
        if (!ValidateSlovakMod11(accountNumber, false))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakAccountNumberChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates Slovak Prefix or Account Number using Weighted Modulo 11.
    /// Weights: 6, 3, 7, 9, 10, 5, 8, 4, 2, 1.
    /// </summary>
    private static bool ValidateSlovakMod11(ReadOnlySpan<char> input, bool isPrefix)
    {
        // Weights array is fixed size 10
        ReadOnlySpan<int> weights = [6, 3, 7, 9, 10, 5, 8, 4, 2, 1];
        
        int sum = 0;
        int inputLength = input.Length;
        
        // If it's a prefix (6 digits), we use the last 6 weights (indices 4-9)
        // If it's an account (10 digits), we use all 10 weights (indices 0-9)
        int weightStartIndex = isPrefix ? 4 : 0;

        for (int i = 0; i < inputLength; i++)
        {
            int digit = input[i] - '0';
            int weight = weights[weightStartIndex + i];
            sum += digit * weight;
        }

        return sum % 11 == 0;
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
