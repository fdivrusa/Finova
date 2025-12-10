using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Slovakia.Validators;

public class SlovakiaIbanValidator : IIbanValidator
{
    public string CountryCode => "SK";
    private const int SlovakiaIbanLength = 24;
    private const string SlovakiaCountryCode = "SK";

    public ValidationResult Validate(string? iban) => ValidateSlovakiaIban(iban);

    public static ValidationResult ValidateSlovakiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SlovakiaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {SlovakiaIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(SlovakiaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected SK.");
        }

        // Structure check: All digits
        for (int i = 2; i < SlovakiaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Slovakia IBAN must contain only digits after the country code.");
            }
        }

        // --- Specific SK Validation (Modulo 11) ---
        // Same algorithm as Czech Republic (CZ)
        string prefix = normalized.Substring(8, 6);
        string accountNumber = normalized.Substring(14, 10);

        // Validate Prefix (only if not all zeros)
        if (long.TryParse(prefix, out long prefixVal) && prefixVal > 0)
        {
            if (!ValidateSlovakMod11(prefix, true))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Slovak Prefix checksum.");
            }
        }

        // Validate Account Number
        if (!ValidateSlovakMod11(accountNumber, false))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Slovak Account Number checksum.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates Slovak Prefix or Account Number using Weighted Modulo 11.
    /// Weights: 6, 3, 7, 9, 10, 5, 8, 4, 2, 1.
    /// </summary>
    private static bool ValidateSlovakMod11(string input, bool isPrefix)
    {
        int[] weights = [6, 3, 7, 9, 10, 5, 8, 4, 2, 1];
        int sum = 0;
        int inputLength = input.Length;
        int weightStartIndex = isPrefix ? 4 : 0;

        for (int i = 0; i < inputLength; i++)
        {
            int digit = input[i] - '0';
            int weight = weights[weightStartIndex + i];
            sum += digit * weight;
        }

        return sum % 11 == 0;
    }
}
