using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Services;

/// <summary>
/// Default implementation of <see cref="INationalIdService"/>.
/// Validates National IDs by delegating to registered <see cref="INationalIdValidator"/> implementations.
/// </summary>
public class NationalIdService : INationalIdService
{
    private readonly IEnumerable<INationalIdValidator> _validators;

    public NationalIdService(IEnumerable<INationalIdValidator> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc/>
    public ValidationResult Validate(string countryCode, string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var validators = _validators.Where(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();

        if (validators.Count == 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, string.Format(ValidationMessages.NoNationalIdValidatorRegistered, countryCode));
        }

        List<ValidationError> errors = [];
        var results = validators.Select(v => v.Validate(nationalId));

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

        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNationalId);
    }
}
