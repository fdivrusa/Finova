using System.Text;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Monaco.Validators;

/// <summary>
/// Validator for Monaco BBAN (Basic Bank Account Number).
/// </summary>
public class MonacoBbanValidator : IBbanValidator
{
    public string CountryCode => "MC";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Monaco BBAN structure and RIB Key.
    /// </summary>
    /// <param name="bban">The BBAN string (23 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 5 digits (Bank) + 5 digits (Branch) + 11 alphanumeric (Account) + 2 digits (RIB Key)
        // Total length: 23 characters

        if (bban.Length != 23)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-5): 5 digits
        for (int i = 0; i < 5; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMonacoIbanBankBranchDigits);
            }
        }

        // 2. Branch Code (Pos 5-10): 5 digits
        for (int i = 5; i < 10; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMonacoIbanBankBranchDigits);
            }
        }

        // 3. Account Number (Pos 10-21: 11 alphanumeric characters
        // (Already implicitly checked by RIB calculation, but good to be explicit if needed)
        // The original code didn't explicitly check alphanumeric for account, but RIB calculation handles letters.

        // 4. RIB Key (Pos 21-23): 2 digits
        if (!char.IsDigit(bban[21]) || !char.IsDigit(bban[22]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMonacoIbanRibKeyDigits);
        }

        // Internal Validation: RIB Key Algorithm
        string bank = bban.Substring(0, 5);
        string branch = bban.Substring(5, 5);
        string account = bban.Substring(10, 11);
        string key = bban.Substring(21, 2);

        if (!ValidateRibKey(bank, branch, account, key))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidMonacoIbanRibKey);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates the Monaco RIB Key.
    /// </summary>
    /// <param name="bank">The bank code.</param>
    /// <param name="branch">The branch code.</param>
    /// <param name="account">The account number.</param>
    /// <param name="key">The RIB key.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool ValidateRibKey(string bank, string branch, string account, string key)
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

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
