using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.China.Validators;

/// <summary>
/// Validates Chinese Unified Social Credit Code (USCC).
/// </summary>
public class ChinaUnifiedSocialCreditCodeValidator : ITaxIdValidator
{
    private static readonly string CharSet = "0123456789ABCDEFGHJKLMNPQRTUWXY";
    private static readonly int[] Weights = { 1, 3, 9, 27, 19, 26, 16, 17, 20, 29, 25, 13, 8, 24, 10, 30, 28 };

    /// <inheritdoc/>
    public string CountryCode => "CN";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateUscc(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Trim().ToUpperInvariant();
        }
        return null;
    }

    /// <summary>
    /// Validates a Chinese Unified Social Credit Code (USCC).
    /// </summary>
    /// <param name="uscc">The USCC string (18 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateUscc(string? uscc)
    {
        if (string.IsNullOrWhiteSpace(uscc))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = uscc.Trim().ToUpperInvariant();

        if (clean.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidUsccLength);
        }

        // Validate characters and calculate checksum
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            int value = CharSet.IndexOf(clean[i]);
            if (value == -1)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidUsccCharacter, clean[i]));
            }
            sum += value * Weights[i];
        }

        int remainder = 31 - (sum % 31);
        if (remainder == 31)
        {
            remainder = 0;
        }

        char expectedCheckDigit = CharSet[remainder];
        if (clean[17] != expectedCheckDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidUsccChecksum);
        }

        return ValidationResult.Success();
    }
}
