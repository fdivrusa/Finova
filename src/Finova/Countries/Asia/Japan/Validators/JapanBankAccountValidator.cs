using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Linq;
using System.Text.RegularExpressions;

namespace Finova.Countries.Asia.Japan.Validators;

/// <summary>
/// Validator for Japan Bank Account Numbers.
/// </summary>
public class JapanBankAccountValidator : IBankAccountValidator, IBankAccountParser
{
    /// <inheritdoc/>
    public string CountryCode => "JP";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators
        string sanitized = Regex.Replace(input, @"[\s-]", "");

        if (!sanitized.All(char.IsDigit))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankAccountMustBeDigits);
        }

        // Japan account numbers are typically 7 digits.
        // Sometimes represented as 8 digits (padded).
        if (sanitized.Length != 7 && sanitized.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidJapanBankAccountLength);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        if (!result.IsValid) return null;
        return Regex.Replace(input!, @"[\s-]", "");
    }

    /// <inheritdoc/>
    public BankAccountDetails? ParseBankAccount(string? accountNumber)
    {
        var result = Validate(accountNumber);
        if (!result.IsValid || string.IsNullOrEmpty(accountNumber))
        {
            return null;
        }

        var normalized = Regex.Replace(accountNumber, @"[\s-]", "");
        
        // Japan format: usually 7 digits.
        // No embedded bank/branch code in the account number itself (usually separate).
        // So we just return the core account number.

        return new BankAccountDetails
        {
            AccountNumber = normalized,
            CountryCode = "JP",
            CoreAccountNumber = normalized
        };
    }
}
