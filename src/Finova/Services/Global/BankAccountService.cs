using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Services;

/// <summary>
/// Default implementation of <see cref="IBankAccountService"/>.
/// Validates Bank Account Numbers by delegating to registered <see cref="IBankAccountValidator"/> implementations.
/// </summary>
public class BankAccountService : IBankAccountService
{
    private readonly IEnumerable<IBankAccountValidator> _validators;
    private readonly IEnumerable<IBankAccountParser> _parsers;

    public BankAccountService(IEnumerable<IBankAccountValidator> validators, IEnumerable<IBankAccountParser> parsers)
    {
        _validators = validators;
        _parsers = parsers;
    }

    /// <inheritdoc/>
    public ValidationResult Validate(string countryCode, string account)
    {
        var validator = _validators.FirstOrDefault(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
        if (validator == null)
        {
            return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"No bank account validator found for country {countryCode}");
        }
        return validator.Validate(account);
    }

    /// <inheritdoc/>
    public BankAccountDetails? Parse(string countryCode, string account)
    {
        var parser = _parsers.FirstOrDefault(p => p.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
        return parser?.ParseBankAccount(account);
    }
}
