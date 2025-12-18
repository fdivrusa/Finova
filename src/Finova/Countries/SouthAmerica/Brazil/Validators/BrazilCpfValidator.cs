using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validates Brazilian CPF (Cadastro de Pessoas Físicas).
/// </summary>
public class BrazilCpfValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "BR";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace(".", "").Replace("-", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a Brazilian CPF (Static).
    /// </summary>
    /// <param name="cpf">The CPF string (e.g., "123.456.789-00").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? cpf)
    {
        return ValidateCpf(cpf);
    }

    /// <summary>
    /// Validates a Brazilian CPF.
    /// </summary>
    /// <param name="cpf">The CPF string (e.g., "123.456.789-00").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators
        var clean = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");

        if (clean.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidCpfLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCpfFormat);
        }

        // Check for all same digits (e.g., 11111111111)
        if (clean.Distinct().Count() == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCpfIdenticalDigits);
        }

        // Validate 1st check digit
        int sum1 = 0;
        for (int i = 0; i < 9; i++)
        {
            sum1 += (clean[i] - '0') * (10 - i);
        }

        int remainder1 = sum1 % 11;
        int digit1 = remainder1 < 2 ? 0 : 11 - remainder1;

        if (digit1 != (clean[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCpfChecksum1);
        }

        // Validate 2nd check digit
        int sum2 = 0;
        for (int i = 0; i < 10; i++)
        {
            sum2 += (clean[i] - '0') * (11 - i);
        }

        int remainder2 = sum2 % 11;
        int digit2 = remainder2 < 2 ? 0 : 11 - remainder2;

        if (digit2 != (clean[10] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCpfChecksum2);
        }

        return ValidationResult.Success();
    }
}
