using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Services;

/// <summary>
/// Default implementation of <see cref="ITaxIdService"/>.
/// Validates Tax IDs by delegating to registered <see cref="ITaxIdValidator"/> implementations.
/// </summary>
public class TaxIdService : ITaxIdService
{
    private readonly IEnumerable<ITaxIdValidator> _validators;

    public TaxIdService(IEnumerable<ITaxIdValidator> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc/>
    public ValidationResult Validate(string countryCode, string? taxId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var validators = _validators.Where(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();

        if (validators.Count == 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, string.Format(ValidationMessages.NoTaxIdValidatorRegistered, countryCode));
        }

        // If multiple validators exist for a country, we return Success if ANY of them passes.
        // If all fail, we return the failure from the first one (or a generic failure).
        List<ValidationError> errors = [];
        foreach (var validator in validators)
        {
            var result = validator.Validate(taxId);
            if (result.IsValid)
            {
                return result;
            }
            errors.AddRange(result.Errors);
        }

        // If we are here, all validators failed.
        // If there was only one, return its result (with errors).
        if (validators.Count == 1)
        {
            return ValidationResult.Failure(errors);
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTaxId);
    }
}
