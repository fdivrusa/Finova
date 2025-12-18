using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Canada.Validators;

/// <summary>
/// Validates Canadian Payments Association (CPA) Routing Numbers.
/// Format: 0AAABBBBB (9 digits) where AAA is Institution Code and BBBBB is Transit Number.
/// </summary>
public class CanadaRoutingNumberValidator : IBankRoutingValidator, IBankRoutingParser
{
    /// <inheritdoc/>
    public string CountryCode => "CA";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates the Canadian Routing Number.
    /// </summary>
    /// <param name="input">The routing number to validate.</param>
    /// <returns>A ValidationResult.</returns>
    public static ValidationResult ValidateStatic(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        if (!Regex.IsMatch(sanitized, @"^0\d{8}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCanadaRoutingNumberFormat);
        }

        // Basic checksum or logic could be added here.
        // For now, length and format.

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
        // Format: 0AAABBBBB
        // AAA = Institution (1-3)
        // BBBBB = Transit (4-8)

        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "CA",
            BankCode = normalized.Substring(1, 3),
            BranchCode = normalized.Substring(4, 5)
        };
    }
}
