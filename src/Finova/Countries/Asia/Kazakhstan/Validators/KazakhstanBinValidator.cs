using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Kazakhstan.Validators;

/// <summary>
/// Validator for Kazakhstan Business Identification Number (BIN) / Individual Identification Number (IIN).
/// Format: 12 digits.
/// </summary>
public class KazakhstanBinValidator : ITaxIdValidator
{
    public string CountryCode => "KZ";

    public ValidationResult Validate(string? input) => ValidateBin(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateBin(string? bin)
    {
        if (string.IsNullOrWhiteSpace(bin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = bin.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("KZ", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "BIN/IIN must be 12 digits.");
        }

        // Checksum calculation
        int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 1, 2 };

        int sum = 0;
        for (int i = 0; i < 11; i++)
        {
            sum += (clean[i] - '0') * weights1[i];
        }
        int control = sum % 11;

        if (control == 10)
        {
            sum = 0;
            for (int i = 0; i < 11; i++)
            {
                sum += (clean[i] - '0') * weights2[i];
            }
            control = sum % 11;
        }

        if (control == 10 || control != (clean[11] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}