using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Oceania.Australia.Validators;

/// <summary>
/// Validates Australian Business Number (ABN).
/// </summary>
public class AustraliaAbnValidator : ITaxIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "AU";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateAbn(input);
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
    /// Validates an Australian ABN.
    /// </summary>
    /// <param name="abn">The ABN string (11 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateAbn(string? abn)
    {
        if (string.IsNullOrWhiteSpace(abn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = abn.Replace(" ", "").Replace("-", "");

        if (clean.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidAbnLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidAbnFormat);
        }

        // Algorithm:
        // 1. Subtract 1 from the first digit.
        // 2. Multiply each digit by its weight.
        // 3. Sum the results.
        // 4. Divide by 89. Remainder must be 0.

        int[] weights = { 10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
        int sum = 0;

        for (int i = 0; i < 11; i++)
        {
            int digit = clean[i] - '0';
            if (i == 0)
            {
                digit -= 1;
            }
            sum += digit * weights[i];
        }

        if (sum % 89 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidAbnChecksum);
        }

        return ValidationResult.Success();
    }
}
