using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Argentina.Validators;

/// <summary>
/// Validates Argentina CUIT/CUIL numbers.
/// </summary>
public class ArgentinaCuitValidator : ITaxIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "AR";

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
    /// Validates an Argentina CUIT/CUIL number.
    /// </summary>
    /// <param name="cuit">The CUIT/CUIL number (11 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = cuit.Trim().Replace("-", "").Replace(" ", "");

        if (clean.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Weights for the first 10 digits
        int[] weights = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        // Calculate weighted sum of first 10 digits
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (clean[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;

        if (checkDigit == 11) checkDigit = 0;

        // If checkDigit is 10, the number is invalid (CUITs cannot have 10 as check digit)
        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        int actualCheckDigit = clean[10] - '0';

        if (actualCheckDigit != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
