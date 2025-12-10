using System.Diagnostics.CodeAnalysis;
using System.Text;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for France bank accounts.
/// France IBAN format: FR + 2 check digits + 5 bank code + 5 branch code + 11 account number + 2 RIB key.
/// </summary>
public class FranceIbanValidator : IIbanValidator
{
    public string CountryCode => "FR";

    private const int FranceIbanLength = 27;
    private const string FranceCountryCode = "FR";

    public ValidationResult Validate(string? iban) => ValidateFranceIban(iban);

    public static ValidationResult ValidateFranceIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != FranceIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {FranceIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(FranceCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected FR.");
        }

        // Indices in normalized string: Bank(4..9), Branch(9..14)
        for (int i = 4; i < 14; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "France Bank/Branch code must be digits.");
            }
        }

        if (!char.IsDigit(normalized[25]) || !char.IsDigit(normalized[26]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "France RIB Key must be digits.");
        }

        // Extract parts
        string bankCode = normalized.Substring(4, 5);
        string branchCode = normalized.Substring(9, 5);
        string accountNumber = normalized.Substring(14, 11);
        string ribKey = normalized.Substring(25, 2);

        if (!ValidateRibKey(bankCode, branchCode, accountNumber, ribKey))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid RIB Key.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates the traditional French RIB Key (Clé RIB).
    /// Formula: Key = 97 - ((89 * Bank + 15 * Branch + 3 * Account) % 97)
    /// </summary>
    private static bool ValidateRibKey(string bank, string branch, string account, string key)
    {
        // Convert Account Number to purely numeric string (replace letters A-Z)
        // French rule: A=1, B=2 ... I=9, J=1, K=2 ... S=1 ... Z=8
        StringBuilder numericAccountBuilder = new();
        foreach (char c in account)
        {
            if (char.IsDigit(c))
            {
                numericAccountBuilder.Append(c);
            }
            else if (char.IsLetter(c))
            {
                // Convert letter to digit based on base-36 subset logic
                // (c - 'A') % 9 + 1 maps A->1, B->2... I->9, J->1, etc.
                int charVal = (char.ToUpperInvariant(c) - 'A') % 9 + 1;
                numericAccountBuilder.Append(charVal);
            }
            else
            {
                return false; // Invalid character in account number
            }
        }

        if (!long.TryParse(bank, out long bankVal) ||
            !long.TryParse(branch, out long branchVal) ||
            !long.TryParse(numericAccountBuilder.ToString(), out long accountVal) ||
            !int.TryParse(key, out int keyVal))
        {
            return false;
        }

        // Calculate the remainder
        // Note: accountVal can be large, but long is sufficient (max 19 digits).
        // Formula components:
        // (89 * Bank) + (15 * Branch) + (3 * Account)
        long total = (89 * bankVal) + (15 * branchVal) + (3 * accountVal);

        long remainder = total % 97;
        long calculatedKey = 97 - remainder;

        return calculatedKey == keyVal;
    }
}
