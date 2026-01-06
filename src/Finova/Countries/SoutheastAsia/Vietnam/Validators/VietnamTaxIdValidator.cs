using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SoutheastAsia.Vietnam.Validators;

/// <summary>
/// Validator for Vietnam Tax Code (Ma So Thue - MST).
/// Format: 10 digits or 13 digits (10 digits + dash + 3 digits).
/// </summary>
public class VietnamTaxIdValidator : ITaxIdValidator
{
    public string CountryCode => "VN";

    public ValidationResult Validate(string? input) => ValidateMst(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Replace("-", "").Trim() : null;

    public static ValidationResult ValidateMst(string? mst)
    {
        if (string.IsNullOrWhiteSpace(mst))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = mst.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("VN", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 10 && clean.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "MST must be 10 or 13 digits.");
        }

        // Validate first 10 digits
        int[] weights = { 31, 29, 23, 19, 17, 13, 7, 5, 3 };
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (clean[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = 10 - remainder;
        if (checkDigit == 10)
        {
            checkDigit = 0;
        }

        if (checkDigit != (clean[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}