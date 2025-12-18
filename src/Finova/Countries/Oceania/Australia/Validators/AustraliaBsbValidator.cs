using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Oceania.Australia.Validators;

/// <summary>
/// Validates Australian Bank State Branch (BSB) numbers.
/// Format: XXY-ZZZ (6 digits).
/// </summary>
public class AustraliaBsbValidator : IBankRoutingValidator, IBankRoutingParser
{
    /// <inheritdoc/>
    public string CountryCode => "AU";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates the BSB number.
    /// </summary>
    /// <param name="input">The BSB number to validate.</param>
    /// <returns>A ValidationResult.</returns>
    public static ValidationResult ValidateStatic(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        if (!Regex.IsMatch(sanitized, @"^\d{6}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBsbFormat);
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
        // Format: XXYZZZ
        // XX = Bank (0-2)
        // Y = State (2-3)
        // ZZZ = Branch (3-6)

        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "AU",
            BankCode = normalized.Substring(0, 2),
            DistrictCode = normalized.Substring(2, 1), // State
            BranchCode = normalized.Substring(3, 3)
        };
    }
}
