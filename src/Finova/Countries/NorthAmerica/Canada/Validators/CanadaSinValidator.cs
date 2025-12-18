using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Canada.Validators;

/// <summary>
/// Validates Canadian Social Insurance Numbers (SIN).
/// </summary>
public class CanadaSinValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CA";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace("-", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a Canadian Social Insurance Number (SIN) (Static).
    /// </summary>
    /// <param name="sin">The SIN string (e.g., "123-456-789").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? sin)
    {
        return ValidateSin(sin);
    }

    /// <summary>
    /// Validates a Canadian Social Insurance Number (SIN).
    /// </summary>
    /// <param name="sin">The SIN string (e.g., "123-456-789").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateSin(string? sin)
    {
        if (string.IsNullOrWhiteSpace(sin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators
        var clean = sin.Replace("-", "").Replace(" ", "");

        if (clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSinLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSinFormat);
        }

        // Validate checksum using Luhn algorithm
        if (!ChecksumHelper.ValidateLuhn(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSinChecksum);
        }

        return ValidationResult.Success();
    }
}
