using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validator for Ireland VAT Number (Enterprise Number).
/// Wraps the existing IrelandVatValidator as the Enterprise Number format is identical to the VAT format.
/// </summary>
public class IrelandVatNumberValidator : ITaxIdValidator
{
    public string CountryCode => "IE";

    public ValidationResult Validate(string? number)
    {
        return ValidateVatNumber(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateVatNumber(string? vatNumber)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(vatNumber, "IE");
        return IrelandVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "IE");
    }
}
