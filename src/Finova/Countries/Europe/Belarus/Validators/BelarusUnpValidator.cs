using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.Belarus.Validators;

/// <summary>
/// Validator for Belarus UNP (Payer's Account Number).
/// Wraps the existing BelarusVatValidator.
/// </summary>
public class BelarusUnpValidator : IEnterpriseValidator
{
    public string CountryCode => "BY";

    public ValidationResult Validate(string? number)
    {
        return ValidateUnp(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateUnp(string? unp)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(unp, "BY");
        return BelarusVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "BY");
    }
}
