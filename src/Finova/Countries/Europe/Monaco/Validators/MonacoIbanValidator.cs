using System.Diagnostics.CodeAnalysis;
using System.Text;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Monaco.Validators;

public class MonacoIbanValidator : IIbanValidator
{
    public string CountryCode => "MC";
    private const int MonacoIbanLength = 27;
    private const string MonacoCountryCode = "MC";

    public ValidationResult Validate(string? iban) => ValidateMonacoIban(iban);

    public static ValidationResult ValidateMonacoIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MonacoIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {MonacoIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(MonacoCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected MC.");
        }

        // Structure check: Bank (5) and Branch (5) must be digits
        for (int i = 4; i < 14; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Monaco Bank/Branch must be digits.");
            }
        }

        // RIB Key (Pos 25-27) must be digits
        if (!char.IsDigit(normalized[25]) || !char.IsDigit(normalized[26]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Monaco RIB Key must be digits.");
        }

        // Internal Validation: RIB Key Algorithm (Same as France)
        string bank = normalized.Substring(4, 5);
        string branch = normalized.Substring(9, 5);
        string account = normalized.Substring(14, 11);
        string key = normalized.Substring(25, 2);

        if (!ValidateRibKey(bank, branch, account, key))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid RIB Key.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    private static bool ValidateRibKey(string bank, string branch, string account, string key)
    {
        // Convert Account Number letters to digits (A=1, B=2... I=9, J=1...)
        StringBuilder numericAccount = new();
        foreach (char c in account)
        {
            if (char.IsDigit(c))
            {
                numericAccount.Append(c);
            }
            else
            {
                // French/Monaco specific mapping
                numericAccount.Append((char.ToUpperInvariant(c) - 'A') % 9 + 1);
            }
        }

        if (!long.TryParse(bank, out long b) ||
            !long.TryParse(branch, out long br) ||
            !long.TryParse(numericAccount.ToString(), out long ac) ||
            !int.TryParse(key, out int k))
        {
            return false;
        }

        // Formula: 97 - ((89*B + 15*G + 3*C) % 97)
        long remainder = (89 * b + 15 * br + 3 * ac) % 97;
        long calculatedKey = 97 - remainder;

        return calculatedKey == k;
    }
}
