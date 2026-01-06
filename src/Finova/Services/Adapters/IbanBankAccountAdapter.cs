using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Identifiers;

namespace Finova.Services.Adapters;

/// <summary>
/// Adapts an <see cref="IIbanValidator"/> to the <see cref="IBankAccountValidator"/> interface.
/// Allows IBAN validators to be used in the global Bank Account service.
/// </summary>
public class IbanBankAccountAdapter : IBankAccountValidator
{
    private readonly IIbanValidator _ibanValidator;

    public IbanBankAccountAdapter(IIbanValidator ibanValidator)
    {
        _ibanValidator = ibanValidator;
    }

    public string CountryCode => _ibanValidator.CountryCode;

    public ValidationResult Validate(string? input)
    {
        return _ibanValidator.Validate(input);
    }

    public string? Parse(string? input)
    {
        var result = _ibanValidator.Validate(input);
        if (!result.IsValid)
        {
            return null;
        }
        // Simple normalization: remove spaces and uppercase
        return input?.Replace(" ", "").ToUpperInvariant();
    }
}