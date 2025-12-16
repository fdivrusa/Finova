using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.China.Validators;

/// <summary>
/// Validates China National Advanced Payment System (CNAPS) code.
/// Format: 12 digits.
/// </summary>
public class ChinaCnapsValidator : IBankRoutingValidator, IBankRoutingParser
{
    /// <inheritdoc/>
    public string CountryCode => "CN";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        if (!Regex.IsMatch(sanitized, @"^\d{12}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCnapsFormat);
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
        // Format: 12 digits
        // Bank: 3 digits (0-3)
        // Region: 4 digits (3-7)
        // Branch: 4 digits (7-11)

        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "CN",
            BankCode = normalized.Substring(0, 3),
            DistrictCode = normalized.Substring(3, 4),
            BranchCode = normalized.Substring(7, 4)
        };
    }
}
