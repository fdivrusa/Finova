using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albania NIPT (Numri i Identifikimit për Personin e Tatueshëm).
/// Wraps the existing AlbaniaVatValidator.
/// </summary>
public class AlbaniaNiptValidator : ITaxIdValidator
{
    public string CountryCode => "AL";

    public ValidationResult Validate(string? number)
    {
        return ValidateNipt(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateNipt(string? nipt)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(nipt, "AL");
        return AlbaniaVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "AL");
    }
}
