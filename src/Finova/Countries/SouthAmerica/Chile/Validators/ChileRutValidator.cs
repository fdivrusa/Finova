using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Chile.Validators;

/// <summary>
/// Validates Chile RUT (Rol Único Tributario) numbers.
/// </summary>
public class ChileRutValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CL";

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
            return input?.Trim().Replace(".", "").Replace("-", "").ToUpperInvariant();
        }
        return null;
    }

    /// <summary>
    /// Validates a Chile RUT number.
    /// </summary>
    /// <param name="rut">The RUT number (8-9 digits including check digit).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? rut)
    {
        if (string.IsNullOrWhiteSpace(rut))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = rut.Trim().Replace(".", "").Replace("-", "").ToUpperInvariant();

        if (clean.Length < 2 || clean.Length > 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Last char is check digit
        char checkDigitChar = clean[^1];
        string body = clean[..^1];

        if (!body.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Calculate check digit
        int sum = 0;
        int multiplier = 2;

        for (int i = body.Length - 1; i >= 0; i--)
        {
            sum += (body[i] - '0') * multiplier;
            multiplier++;
            if (multiplier > 7) multiplier = 2;
        }

        int remainder = 11 - (sum % 11);
        char calculatedCheckDigit;

        if (remainder == 11) calculatedCheckDigit = '0';
        else if (remainder == 10) calculatedCheckDigit = 'K';
        else calculatedCheckDigit = (char)(remainder + '0');

        if (checkDigitChar != calculatedCheckDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
