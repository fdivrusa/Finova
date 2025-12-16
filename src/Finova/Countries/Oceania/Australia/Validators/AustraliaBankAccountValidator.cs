using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Oceania.Australia.Validators;

/// <summary>
/// Validates Australian Bank Account Numbers.
/// Format: 6-9 digits (sometimes up to 10 depending on bank, but standard is 6-9).
/// </summary>
public class AustraliaBankAccountValidator : IBankAccountValidator, IBankAccountParser
{
    /// <inheritdoc/>
    public string CountryCode => "AU";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        // Australian account numbers are typically 6-9 digits.
        // Some legacy systems might have fewer, but 6-9 is the standard range for validation.
        if (sanitized.Length < 6 || sanitized.Length > 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!Regex.IsMatch(sanitized, @"^\d+$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace(" ", "").Replace("-", "");
        }
        return null;
    }

    /// <inheritdoc/>
    public BankAccountDetails? ParseBankAccount(string? accountNumber)
    {
        var result = Validate(accountNumber);
        if (!result.IsValid || string.IsNullOrEmpty(accountNumber))
        {
            return null;
        }

        var normalized = accountNumber.Replace(" ", "").Replace("-", "");

        return new BankAccountDetails
        {
            AccountNumber = normalized,
            CountryCode = "AU"
        };
    }
}
