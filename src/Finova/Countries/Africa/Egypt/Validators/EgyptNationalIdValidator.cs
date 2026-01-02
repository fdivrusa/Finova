using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Globalization;

namespace Finova.Countries.Africa.Egypt.Validators;

/// <summary>
/// Validates Egyptian National ID numbers.
/// </summary>
public class EgyptNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "EG";

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
    /// Validates an Egyptian National ID number.
    /// </summary>
    /// <param name="idNumber">The ID number (14 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = idNumber.Trim().Replace(" ", "");

        if (clean.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        int century = clean[0] - '0';
        if (century != 2 && century != 3)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        string datePart = clean.Substring(1, 6);
        if (!DateTime.TryParseExact(datePart, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        int sum = 0;
        int[] weights = { 2, 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        for (int i = 0; i < 13; i++)
        {
            sum += (clean[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit == 10) checkDigit = 0;

        if (checkDigit == 11) checkDigit = 0;
        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        if (checkDigit != (clean[13] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
