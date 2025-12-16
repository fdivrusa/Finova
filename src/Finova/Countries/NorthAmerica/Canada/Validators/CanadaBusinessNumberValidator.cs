using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Canada.Validators;

/// <summary>
/// Validates Canadian Business Numbers (BN).
/// </summary>
public class CanadaBusinessNumberValidator : ITaxIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CA";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateBn(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            var clean = input?.Replace("-", "").Replace(" ", "");
            if (clean != null && clean.Length > 9)
            {
                return clean.Substring(0, 9);
            }
            return clean;
        }
        return null;
    }

    /// <summary>
    /// Validates a Canadian Business Number (BN).
    /// </summary>
    /// <param name="bn">The BN string (e.g., "123456789").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateBn(string? bn)
    {
        if (string.IsNullOrWhiteSpace(bn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators
        var clean = bn.Replace("-", "").Replace(" ", "");

        // BN is 9 digits (the root). If input contains suffix (e.g. RC0001), validate the root.
        if (clean.Length > 9)
        {
            clean = clean.Substring(0, 9);
        }

        if (clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBnLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBnFormat);
        }

        // The BN is validated using the Luhn formula (same as SIN).
        if (!ChecksumHelper.ValidateLuhn(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidBnChecksum);
        }

        return ValidationResult.Success();
    }
}
