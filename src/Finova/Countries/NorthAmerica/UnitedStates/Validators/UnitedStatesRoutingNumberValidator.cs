using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.UnitedStates.Validators;

/// <summary>
/// Validates United States ABA Routing Numbers (RTN).
/// </summary>
public class UnitedStatesRoutingNumberValidator : IBankRoutingValidator, IBankRoutingParser
{
    /// <inheritdoc/>
    public string CountryCode => "US";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateRoutingNumber(input);
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
        // ABA Routing Number format:
        // First 4 digits: Federal Reserve Routing Symbol
        // Next 4 digits: ABA Institution Identifier
        // Last digit: Check Digit

        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "US",
            DistrictCode = normalized.Substring(0, 4),
            BankCode = normalized.Substring(4, 4),
            CheckDigits = normalized.Substring(8, 1)
        };
    }

    /// <summary>
    /// Validates a US ABA Routing Number.
    /// </summary>
    /// <param name="routingNumber">The 9-digit routing number.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateRoutingNumber(string? routingNumber)
    {
        if (string.IsNullOrWhiteSpace(routingNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove spaces and dashes
        var clean = routingNumber.Replace(" ", "").Replace("-", "");

        if (clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidRoutingNumberLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidRoutingNumberFormat);
        }

        // ABA Routing Number Checksum Algorithm
        // 3 * (d1 + d4 + d7) + 7 * (d2 + d5 + d8) + 1 * (d3 + d6 + d9) mod 10 == 0
        int sum = 0;
        for (int i = 0; i < 9; i += 3)
        {
            sum += 3 * (clean[i] - '0');
            sum += 7 * (clean[i + 1] - '0');
            sum += 1 * (clean[i + 2] - '0');
        }

        if (sum % 10 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidRoutingNumberChecksum);
        }

        return ValidationResult.Success();
    }
}
