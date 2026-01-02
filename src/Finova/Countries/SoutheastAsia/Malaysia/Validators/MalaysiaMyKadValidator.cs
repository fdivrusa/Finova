using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Globalization;

namespace Finova.Countries.SoutheastAsia.Malaysia.Validators;

/// <summary>
/// Validates Malaysian MyKad numbers.
/// </summary>
public class MalaysiaMyKadValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "MY";

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
            return input?.Trim().Replace("-", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a Malaysian MyKad number.
    /// </summary>
    /// <param name="idNumber">The ID number (12 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = idNumber.Trim().Replace("-", "").Replace(" ", "");

        if (clean.Length != 12)
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

        // Place of Birth (PB) - digits 7-8
        // Just check numeric (already done)

        return ValidationResult.Success();
    }
}
