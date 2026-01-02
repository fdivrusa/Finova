using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Globalization;

namespace Finova.Countries.Africa.SouthAfrica.Validators;

/// <summary>
/// Validates South African ID numbers.
/// </summary>
public class SouthAfricaIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "ZA";

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
            return input?.Trim().Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a South African ID number.
    /// </summary>
    /// <param name="idNumber">The ID number (13 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = idNumber.Trim().Replace(" ", "");

        if (clean.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Date (YYMMDD)
        string datePart = clean.Substring(0, 6);
        if (!DateTime.TryParseExact(datePart, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Checksum (Luhn)
        if (!ChecksumHelper.ValidateLuhn(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
