using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Globalization;

namespace Finova.Countries.SoutheastAsia.Indonesia.Validators;

/// <summary>
/// Validates Indonesian NIK (Nomor Induk Kependudukan) numbers.
/// </summary>
public class IndonesiaNikValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "ID";

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
            return input?.Trim().Replace(".", "").Replace(" ", "").Replace("-", "");
        }
        return null;
    }

    /// <summary>
    /// Validates an Indonesian NIK number.
    /// </summary>
    /// <param name="nik">The NIK number (16 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nik)
    {
        if (string.IsNullOrWhiteSpace(nik))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nik.Trim().Replace(".", "").Replace(" ", "").Replace("-", "");

        if (clean.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Date (DDMMYY) - digits 7-12
        // Females have DD + 40
        string datePart = clean.Substring(6, 6);
        int day = int.Parse(datePart.Substring(0, 2));
        int month = int.Parse(datePart.Substring(2, 2));
        int year = int.Parse(datePart.Substring(4, 2));

        if (day > 40)
        {
            day -= 40;
        }

        string adjustedDate = $"{day:D2}{month:D2}{year:D2}";
        if (!DateTime.TryParseExact(adjustedDate, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return ValidationResult.Success();
    }
}
