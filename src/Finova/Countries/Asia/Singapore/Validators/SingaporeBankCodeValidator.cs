using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Singapore.Validators;

/// <summary>
/// Validates Singapore Bank Code and Branch Code.
/// Format: BBB-BBBBB (3-digit bank code + 5-digit branch code) or codes separately.
/// </summary>
public class SingaporeBankCodeValidator : IBankRoutingValidator, IBankRoutingParser
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

        string sanitized = input.Replace(" ", "").Replace("-", "");

        // Singapore bank codes: 
        // - Bank code: 4 digits (SWIFT bank identifier)
        // - Branch code: 3 digits
        // - Combined: 7 digits (typically in format BBBB-BBB)
        if (!Regex.IsMatch(sanitized, @"^\d{3,7}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSingaporeBankCodeFormat);
        }

        // Accept 3-digit branch code, 4-digit bank code, or 7-digit combined
        if (sanitized.Length != 3 && sanitized.Length != 4 && sanitized.Length != 7)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSingaporeBankCodeLength);
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
    public BankRoutingDetails? ParseRoutingNumber(string? routingNumber)
    {
        var result = Validate(routingNumber);
        if (!result.IsValid || string.IsNullOrEmpty(routingNumber))
        {
            return null;
        }

        var normalized = routingNumber.Replace(" ", "").Replace("-", "");

        string? bankCode = null;
        string? branchCode = null;

        if (normalized.Length == 7)
        {
            // Combined format: BBBB-BBB
            bankCode = normalized.Substring(0, 4);
            branchCode = normalized.Substring(4, 3);
        }
        else if (normalized.Length == 4)
        {
            // Bank code only
            bankCode = normalized;
        }
        else if (normalized.Length == 3)
        {
            // Branch code only
            branchCode = normalized;
        }

        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "SG",
            BankCode = bankCode,
            BranchCode = branchCode
        };
    }
}
