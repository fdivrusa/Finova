using Finova.Core.Bic;
using Finova.Services;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class IbanValidators
{
    /// <summary>
    /// Validates that the string is a valid IBAN.
    /// Automatically detects the country and applies the correct rules (e.g. Italy vs Germany).
    /// </summary>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Iban).MustBeValidIban();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> MustBeValidIban<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(iban => EuropeIbanValidator.ValidateIban(iban).IsValid)
            .WithMessage("'{PropertyName}' is not a valid IBAN.");
    }

    /// <summary>
    /// Validates that the string is a valid BIC/SWIFT code (ISO 9362).
    /// </summary>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Bic).MustBeValidBic();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> MustBeValidBic<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(x => BicValidator.Validate(x).IsValid)
            .WithMessage("'{PropertyName}' is not a valid BIC/SWIFT code.");
    }

    /// <summary>
    /// Validates that the BIC country code matches the IBAN country code.
    /// </summary>
    /// <param name="ibanSelector">Expression to select the IBAN property to compare against.</param>
    public static IRuleBuilderOptions<T, string?> MustMatchIbanCountry<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> ibanSelector)
    {
        return ruleBuilder
            .Must((rootObject, bic) =>
            {
                var iban = ibanSelector(rootObject);
                if (string.IsNullOrWhiteSpace(iban))
                {
                    return true;
                }

                return BicValidator.ValidateConsistencyWithIban(bic, iban[..2]).IsValid;
            })
            .WithMessage("The BIC country does not match the IBAN country.");
    }
}
