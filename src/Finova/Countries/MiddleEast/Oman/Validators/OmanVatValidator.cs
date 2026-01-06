using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.Oman.Validators;

/// <summary>
/// Validator for Oman VAT Number.
/// Format: 15 digits. Starts with 'OM'.
/// </summary>
public class OmanVatValidator : ITaxIdValidator, IVatValidator
{
    public string CountryCode => "OM";

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
            CountryCode = "OM",
            VatNumber = input!.Trim().ToUpperInvariant(),
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

        var clean = vat.Trim().ToUpperInvariant();
        if (clean.StartsWith("OM"))
        {
            clean = clean[2..];
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 13) // Updated to match example length (2+13=15)
        {
            if (vat.Trim().Length != 15)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Oman VAT number must be 15 characters.");
            }
        }

        return ValidationResult.Success();
    }
}