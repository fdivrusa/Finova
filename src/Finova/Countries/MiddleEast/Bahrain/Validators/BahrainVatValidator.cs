using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.Bahrain.Validators;

/// <summary>
/// Validator for Bahrain VAT Number / Tax Registration Number (TRN).
/// Format: 15 digits. Usually starts with 3.
/// </summary>
public class BahrainVatValidator : ITaxIdValidator, IVatValidator
{
    public string CountryCode => "BH";

    public ValidationResult Validate(string? input) => ValidateVat(input);

    /// <summary>
    /// Explicit implementation for IVatValidator.
    /// </summary>
    VatDetails? IValidator<VatDetails>.Parse(string? input)
    {
        var result = Validate(input);
        if (!result.IsValid)
        {
            return null;
        }

        return new VatDetails
        {
            CountryCode = "BH",
            VatNumber = input!.Trim(),
            IsValid = true,
            IdentifierKind = "VAT"
        };
    }

    /// <summary>
    /// Implementation for ITaxIdValidator / IValidator&lt;string&gt;.
    /// </summary>
    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim().ToUpperInvariant() : null;

    public static ValidationResult ValidateVat(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Bahrain VAT number must be 15 digits.");
        }

        if (!clean.StartsWith('3'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bahrain VAT number typically starts with '3'.");
        }

        return ValidationResult.Success();
    }
}