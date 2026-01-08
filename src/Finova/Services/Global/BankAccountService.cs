using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Services;

/// <summary>
/// Default implementation of <see cref="IBankAccountService"/>.
/// Validates Bank Account Numbers by delegating to registered <see cref="IBankAccountValidator"/> implementations.
/// </summary>
public class BankAccountService(IEnumerable<IBankAccountValidator> validators, IEnumerable<IBankAccountParser> parsers) : IBankAccountService
{
    private readonly IEnumerable<IBankAccountValidator> _validators = validators;
    private readonly IEnumerable<IBankAccountParser> _parsers = parsers;

    /// <inheritdoc/>
    public ValidationResult Validate(string countryCode, string account)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var validators = _validators.Where(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();

        if (validators.Count == 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"No bank account validator found for country {countryCode}");
        }

        List<ValidationError> errors = [];
        var results = validators.Select(v => v.Validate(account));

        foreach (var result in results)
        {
            if (result.IsValid)
            {
                return result;
            }
            errors.AddRange(result.Errors);
        }

        if (validators.Count == 1)
        {
            return ValidationResult.Failure(errors);
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid bank account number");
    }

    /// <inheritdoc/>
    public BankAccountDetails? Parse(string countryCode, string account)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return null;
        }

        var parsers = _parsers.Where(p => p.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();

        if (parsers.Count == 0)
        {
            return null;
        }

        var result = parsers.Select(parser => parser.ParseBankAccount(account))
                            .FirstOrDefault(r => r != null);

        if (result != null)
        {
            return result;
        }

        return null;
    }
}
