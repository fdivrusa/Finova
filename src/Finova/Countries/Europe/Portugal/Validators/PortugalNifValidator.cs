using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Portugal.Validators;

/// <summary>
/// Validator for Portugal NIF (Número de Identificação Fiscal).
/// Wraps the existing PortugalVatValidator as the NIF format is identical to the VAT format.
/// </summary>
public class PortugalNifValidator : ITaxIdValidator
{
    public string CountryCode => "PT";

    public ValidationResult Validate(string? number)
    {
        return ValidateNif(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateNif(string? nif)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(nif, "PT");
        return PortugalVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "PT");
    }
}
