using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.India.Validators;

/// <summary>
/// Validates Indian Financial System Code (IFSC).
/// Format: AAAA0BBBBBB (11 characters).
/// </summary>
public class IndiaIfscValidator : IBankRoutingValidator, IBankRoutingParser
{
    /// <inheritdoc/>
    public string CountryCode => "IN";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").ToUpper();

        if (!Regex.IsMatch(sanitized, @"^[A-Z]{4}0[A-Z0-9]{6}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIfscFormat);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace(" ", "").ToUpper();
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

        var normalized = routingNumber.Replace(" ", "").ToUpper();
        // Format: AAAA0BBBBBB
        // AAAA = Bank (0-4)
        // BBBBBB = Branch (5-11)

        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "IN",
            BankCode = normalized.Substring(0, 4),
            BranchCode = normalized.Substring(5, 6)
        };
    }
}
