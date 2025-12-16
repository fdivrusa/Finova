using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Enterprise;

namespace Finova.Services.Adapters;

/// <summary>
/// Adapts an <see cref="IEnterpriseValidator"/> to the <see cref="ITaxIdValidator"/> interface.
/// Allows European Enterprise validators (SIRET, KBO, etc.) to be used in the global Tax ID service.
/// </summary>
public class EuropeEnterpriseTaxIdAdapter : ITaxIdValidator
{
    private readonly IEnterpriseValidator _enterpriseValidator;

    public EuropeEnterpriseTaxIdAdapter(IEnterpriseValidator enterpriseValidator)
    {
        _enterpriseValidator = enterpriseValidator;
    }

    public string CountryCode => _enterpriseValidator.CountryCode;

    public ValidationResult Validate(string? input)
    {
        return _enterpriseValidator.Validate(input);
    }

    public string? Parse(string? input)
    {
        return _enterpriseValidator.Parse(input);
    }
}
