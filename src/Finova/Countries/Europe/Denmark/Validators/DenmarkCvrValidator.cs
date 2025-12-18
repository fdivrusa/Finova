using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Denmark.Validators;

/// <summary>
/// Validator for Denmark CVR (Centrale Virksomhedsregister).
/// Wraps the existing DenmarkVatValidator.
/// </summary>
public class DenmarkCvrValidator : ITaxIdValidator
{
    public string CountryCode => "DK";

    public ValidationResult Validate(string? number)
    {
        return ValidateCvr(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateCvr(string? cvr)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(cvr, "DK");
        return DenmarkVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "DK");
    }
}
