using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Services.Adapters;

/// <summary>
/// Adapts an <see cref="IVatValidator"/> to the <see cref="ITaxIdValidator"/> interface.
/// Allows European VAT validators to be used in the global Tax ID service.
/// </summary>
public class EuropeVatTaxIdAdapter : ITaxIdValidator
{
    private readonly IVatValidator _vatValidator;

    public EuropeVatTaxIdAdapter(IVatValidator vatValidator)
    {
        _vatValidator = vatValidator;
    }

    public string CountryCode => _vatValidator.CountryCode;

    public ValidationResult Validate(string? input)
    {
        return _vatValidator.Validate(input);
    }

    public string? Parse(string? input)
    {
        var details = _vatValidator.Parse(input);
        return details?.ToString(); // Returns the formatted VAT number
    }
}
