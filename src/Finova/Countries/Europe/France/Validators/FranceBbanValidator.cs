using System.Text;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French BBAN (Basic Bank Account Number).
/// Format: Bank (5) + Branch (5) + Account (11) + Key (2).
/// </summary>
public class FranceBbanValidator : IBbanValidator
{
    private const int FranceBbanLength = 23;

    public string CountryCode => "FR";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the French BBAN.
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

        if (bban.Length != FranceBbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, FranceBbanLength, bban.Length));
        }

        // Indices in BBAN string: Bank(0..5), Branch(5..10), Account(10..21), Key(21..23)
        for (int i = 0; i < 10; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.FranceBankBranchCodeMustBeDigits);
            }
        }

        if (!char.IsDigit(bban[21]) || !char.IsDigit(bban[22]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.FranceRibKeyMustBeDigits);
        }

        // Extract parts
        string bankCode = bban.Substring(0, 5);
        string branchCode = bban.Substring(5, 5);
        string accountNumber = bban.Substring(10, 11);
        string ribKey = bban.Substring(21, 2);

        if (!ValidateRibKey(bankCode, branchCode, accountNumber, ribKey))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidRibKey);
        }

        return ValidationResult.Success();
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

        // Algorithm:
        // 1. Calculate weighted sum
        // 2. Modulo 97
        // 3. Subtract from 97
        // 4. Compare with key

        // Using BigInteger or careful long arithmetic to avoid overflow is good, 
        // but standard long is enough here because:
        // Bank(5) ~ 99999 * 89 ~ 8.9M
        // Branch(5) ~ 99999 * 15 ~ 1.5M
        // Account(11) ~ 99B * 3 ~ 300B
        // Total sum fits in long (max 9e18).

        long remainder = (89 * bankVal + 15 * branchVal + 3 * accountVal) % 97;
        long calculatedKey = 97 - remainder;

        return calculatedKey == keyVal;
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string sanitized = input.Replace(" ", "").Replace("-", "").Trim();
        return Validate(sanitized).IsValid ? sanitized : null;
    }
}
