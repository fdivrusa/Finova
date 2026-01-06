using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Singapore.Validators;

/// <summary>
/// Validator for Singapore Bank Account Numbers.
/// </summary>
public class SingaporeBankAccountValidator : IBankAccountValidator, IBankAccountParser
{
    /// <inheritdoc/>
    public string CountryCode => "SG";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators (spaces, dashes)
        string sanitized = Regex.Replace(input, @"[\s-]", "");

        if (!sanitized.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankAccountMustBeDigits);
        }

        // Common length is 10 digits (including bank/branch codes often) or just account number.
        // We allow a range to cover various banks.
        if (sanitized.Length < 7 || sanitized.Length > 17)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSingaporeBankAccountLength);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        if (!result.IsValid)
        {
            return null;
        }

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

        // Singapore format varies by bank (DBS, OCBC, UOB).
        // Often: Branch (3) + Account (7-9).
        // Without specific bank context, we can try to extract branch if length permits.

        string? branchCode = null;
        string? coreAccount = normalized;

        if (normalized.Length >= 10)
        {
            // Heuristic: First 3 digits are often branch code
            branchCode = normalized.Substring(0, 3);
            coreAccount = normalized.Substring(3);
        }

        return new BankAccountDetails
        {
            AccountNumber = normalized,
            CountryCode = "SG",
            BranchCode = branchCode,
            CoreAccountNumber = coreAccount
        };
    }
}
