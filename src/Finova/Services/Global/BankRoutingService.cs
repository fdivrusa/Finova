using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Services;

/// <summary>
/// Default implementation of <see cref="IBankRoutingService"/>.
/// Validates Bank Routing Numbers by delegating to registered <see cref="IBankRoutingValidator"/> implementations.
/// </summary>
public class BankRoutingService : IBankRoutingService
{
    private readonly IEnumerable<IBankRoutingValidator> _validators;
    private readonly IEnumerable<IBankRoutingParser> _parsers;

    public BankRoutingService(IEnumerable<IBankRoutingValidator> validators, IEnumerable<IBankRoutingParser> parsers)
    {
        _validators = validators;
        _parsers = parsers;
    }

    /// <inheritdoc/>
    public ValidationResult Validate(string countryCode, string? routingNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var validators = _validators.Where(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();

        if (validators.Count == 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, string.Format(ValidationMessages.NoBankRoutingValidatorRegistered, countryCode));
        }

        List<ValidationError> errors = [];
        foreach (var validator in validators)
        {
            var result = validator.Validate(routingNumber);
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

        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBankRoutingNumber);
    }

    /// <inheritdoc/>
    public BankRoutingDetails? Parse(string countryCode, string? routingNumber)
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

        foreach (var parser in parsers)
        {
            var result = parser.ParseRoutingNumber(routingNumber);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
