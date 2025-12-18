using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Andorra.Validators;

/// <summary>
/// Validator for Andorra NRT (Número de Registre Tributari).
/// Wraps the existing AndorraVatValidator.
/// </summary>
public class AndorraNrtValidator : ITaxIdValidator
{
    public string CountryCode => "AD";

    public ValidationResult Validate(string? number)
    {
        return ValidateNrt(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateNrt(string? nrt)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(nrt, "AD");
        return AndorraVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(instance, "AD");
        if (normalized == null || normalized.Length != 8)
        {
            return normalized;
        }
        // F-123456-Z
        return $"{normalized[0]}-{normalized.Substring(1, 6)}-{normalized[7]}";
    }
}
