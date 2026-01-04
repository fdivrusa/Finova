using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Japan.Validators;

/// <summary>
/// Validates Japanese Zengin Bank/Branch Codes.
/// Format: BBBB-BBB (4-digit bank code + 3-digit branch code) or 7 digits combined.
/// </summary>
public class JapanZenginCodeValidator : IBankRoutingValidator, IBankRoutingParser
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

        string sanitized = input.Replace(" ", "").Replace("-", "");

        // Japanese Zengin codes: 4-digit bank code + 3-digit branch code = 7 digits total
        // Bank code alone: 4 digits
        // Branch code alone: 3 digits
        // Combined: 7 digits
        if (!Regex.IsMatch(sanitized, @"^\d{3,7}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidJapanZenginCodeFormat);
        }

        // Accept 3-digit branch code, 4-digit bank code, or 7-digit combined
        if (sanitized.Length != 3 && sanitized.Length != 4 && sanitized.Length != 7)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidJapanZenginCodeLength);
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

        // Parse based on length
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
            CountryCode = "JP",
            BankCode = bankCode,
            BranchCode = branchCode
        };
    }
}
