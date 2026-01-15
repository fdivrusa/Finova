using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Egypt.Validators;

/// <summary>
/// Validator for Egyptian Tax Registration Number (TRN).
/// Format: 9 digits.
/// </summary>
public class EgyptTaxRegistrationNumberValidator : ITaxIdValidator
{
    public string CountryCode => "EG";

    public ValidationResult Validate(string? input) => ValidateTrn(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Replace("-", "").Trim() : null;

    public static ValidationResult ValidateTrn(string? trn)
    {
        if (string.IsNullOrWhiteSpace(trn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = trn.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("EG", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidEgyptTrnLength);
        }

        return ValidationResult.Success();
    }
}