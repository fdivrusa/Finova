using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Norway.Validators;

public class NorwayIbanValidator : IIbanValidator
{
    public string CountryCode => "NO";
    private const int NorwayIbanLength = 15;
    private const string NorwayCountryCode = "NO";

    public ValidationResult Validate(string? iban) => ValidateNorwayIban(iban);

    public static ValidationResult ValidateNorwayIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != NorwayIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {NorwayIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(NorwayCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected NO.");
        }

        // Structure check: All body characters must be digits
        for (int i = 2; i < NorwayIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Norway IBAN must contain only digits after the country code.");
            }
        }

        // Internal Validation: Modulo 11 on BBAN (Last 11 digits)
        // Indices 4 to 15 in IBAN
        string bban = normalized.Substring(4, 11);
        if (!ValidateMod11(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Norwegian BBAN checksum.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates Norwegian BBAN using Modulo 11 with weights.
    /// Weights sequence: 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 on the first 10 digits.
    /// </summary>
    private static bool ValidateMod11(string bban)
    {
        // BBAN is 11 digits. Last one is check digit.
        int[] weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
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
}
