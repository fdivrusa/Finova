using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Oceania.Australia.Validators;

/// <summary>
/// Validates Australian Tax File Number (TFN).
/// </summary>
public class AustraliaTfnValidator : ITaxIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "AU";

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
            return input?.Replace(" ", "").Replace("-", "");
        }
        return null;
    }

    /// <summary>
    /// Validates an Australian TFN (Static).
    /// </summary>
    /// <param name="tfn">The TFN string (8 or 9 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? tfn)
    {
        return ValidateTfn(tfn);
    }

    /// <summary>
    /// Validates an Australian TFN.
    /// </summary>
    /// <param name="tfn">The TFN string (8 or 9 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateTfn(string? tfn)
    {
        if (string.IsNullOrWhiteSpace(tfn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = tfn.Replace(" ", "").Replace("-", "");

        if (clean.Length != 8 && clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidTfnLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTfnFormat);
        }

        int sum = 0;
        if (clean.Length == 9)
        {
            int[] weights = { 1, 4, 3, 7, 5, 8, 6, 9, 10 };
            for (int i = 0; i < 9; i++)
            {
                sum += (clean[i] - '0') * weights[i];
            }
        }
        else // 8 digits
        {
            int[] weights = { 10, 7, 8, 4, 6, 3, 5, 1 };
            for (int i = 0; i < 8; i++)
            {
                sum += (clean[i] - '0') * weights[i];
            }
        }

        if (sum % 11 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidTfnChecksum);
        }

        return ValidationResult.Success();
    }
}
