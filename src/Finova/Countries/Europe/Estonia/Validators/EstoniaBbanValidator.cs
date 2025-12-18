using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Estonia.Validators;

/// <summary>
/// Validator for Estonian BBAN (Basic Bank Account Number).
/// </summary>
public class EstoniaBbanValidator : IBbanValidator
{
    public string CountryCode => "EE";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Estonian BBAN using the 7-3-1 method.
    /// </summary>
    /// <param name="bban">The BBAN to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 16, bban.Length));
        }

        // Weights: 7, 3, 1 repeating
        int[] weights = [7, 3, 1];
        int sum = 0;

        // Iterate over the first 15 digits (last one is check digit)
        for (int i = 0; i < 15; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.EstoniaIbanMustBeDigits);
            }

            int digit = bban[i] - '0';
            // Determine weight based on position (cycling 0, 1, 2)
            int weight = weights[i % 3];
            sum += digit * weight;
        }

        if (!char.IsDigit(bban[15]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.EstoniaIbanMustBeDigits);
        }

        // Calculate Check Digit
        // Formula: (10 - (Sum % 10)) % 10
        int remainder = sum % 10;
        int checkDigit = (remainder == 0) ? 0 : 10 - remainder;

        if (checkDigit != (bban[15] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidEstoniaBbanStructure);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
