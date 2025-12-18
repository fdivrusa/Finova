using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validates Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica).
/// </summary>
public class BrazilCnpjValidator : ITaxIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "BR";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateCnpj(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a Brazilian CNPJ.
    /// </summary>
    /// <param name="cnpj">The CNPJ string (e.g., "12.345.678/0001-90").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateCnpj(string? cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators
        var clean = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");

        if (clean.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidCnpjLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCnpjFormat);
        }

        // Check for all same digits
        if (clean.Distinct().Count() == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCnpjIdenticalDigits);
        }

        // Validate 1st check digit
        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum1 = 0;
        for (int i = 0; i < 12; i++)
        {
            sum1 += (clean[i] - '0') * weights1[i];
        }

        int remainder1 = sum1 % 11;
        int digit1 = remainder1 < 2 ? 0 : 11 - remainder1;

        if (digit1 != (clean[12] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCnpjChecksum1);
        }

        // Validate 2nd check digit
        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum2 = 0;
        for (int i = 0; i < 13; i++)
        {
            sum2 += (clean[i] - '0') * weights2[i];
        }

        int remainder2 = sum2 % 11;
        int digit2 = remainder2 < 2 ? 0 : 11 - remainder2;

        if (digit2 != (clean[13] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCnpjChecksum2);
        }

        return ValidationResult.Success();
    }
}
