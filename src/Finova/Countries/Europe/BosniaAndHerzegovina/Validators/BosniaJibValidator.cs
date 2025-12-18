using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

/// <summary>
/// Validator for Bosnia and Herzegovina JIB (Jedinstveni Identifikacioni Broj).
/// Wraps the existing BosniaAndHerzegovinaVatValidator.
/// </summary>
public class BosniaJibValidator : ITaxIdValidator
{
    public string CountryCode => "BA";

    public ValidationResult Validate(string? number)
    {
        return ValidateJib(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateJib(string? jib)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(jib, "BA");
        return BosniaAndHerzegovinaVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "BA");
    }
}
