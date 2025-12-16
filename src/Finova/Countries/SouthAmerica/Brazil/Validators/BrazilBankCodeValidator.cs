using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validates Brazil Bank Code (Código de Compensação - COMPE).
/// Format: 3 digits.
/// </summary>
public class BrazilBankCodeValidator : IBankRoutingValidator, IBankRoutingParser
{
    /// <inheritdoc/>
    public string CountryCode => "BR";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        if (!Regex.IsMatch(sanitized, @"^\d{3}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBrazilBankCodeFormat);
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
        // Format: 3 digits

        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "BR",
            BankCode = normalized
        };
    }
}
