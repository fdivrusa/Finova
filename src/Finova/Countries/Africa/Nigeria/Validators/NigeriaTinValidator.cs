using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Nigeria.Validators;

/// <summary>
/// Validator for Nigerian Tax Identification Number (TIN).
/// Format: usually 8-12 digits.
/// </summary>
public class NigeriaTinValidator : ITaxIdValidator
{
    public string CountryCode => "NG";

    public ValidationResult Validate(string? input) => ValidateTin(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateTin(string? tin)
    {
        if (string.IsNullOrWhiteSpace(tin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = tin.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("NG", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Accepting 8 to 12 digits to be safe.
        if (clean.Length < 8 || clean.Length > 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidNigeriaTinLength);
        }

        return ValidationResult.Success();
    }
}