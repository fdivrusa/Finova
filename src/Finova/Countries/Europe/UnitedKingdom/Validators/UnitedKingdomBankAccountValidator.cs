using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validates United Kingdom Bank Account Numbers.
/// Format: 8 digits.
/// </summary>
public class UnitedKingdomBankAccountValidator : IBankAccountValidator, IBankAccountParser
{
    /// <inheritdoc/>
    public string CountryCode => "GB";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        if (sanitized.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!Regex.IsMatch(sanitized, @"^\d{8}$"))
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
            CountryCode = "GB"
        };
    }
}
