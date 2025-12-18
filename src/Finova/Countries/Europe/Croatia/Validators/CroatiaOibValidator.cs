using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Croatia.Validators;

/// <summary>
/// Validator for Croatia OIB (Osobni identifikacijski broj).
/// Wraps the existing CroatiaVatValidator.
/// </summary>
public class CroatiaOibValidator : ITaxIdValidator, INationalIdValidator
{
    public string CountryCode => "HR";

    public ValidationResult Validate(string? number)
    {
        return ValidateStatic(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateStatic(string? number)
    {
        return ValidateOib(number);
    }

    public static ValidationResult ValidateOib(string? oib)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(oib, "HR");
        return CroatiaVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "HR");
    }
}
