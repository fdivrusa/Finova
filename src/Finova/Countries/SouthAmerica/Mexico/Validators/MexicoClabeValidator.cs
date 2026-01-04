using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Mexico.Validators;

/// <summary>
/// Validates Mexican CLABE (Clave Bancaria Estandarizada).
/// Format: 18 digits - BBB-BBB-AAAAAAAAAAA-C
/// Where: BBB = Bank code (3 digits), BBB = Branch code (3 digits), 
/// AAAAAAAAAAA = Account number (11 digits), C = Check digit (1 digit).
/// </summary>
public class MexicoClabeValidator : IBankRoutingValidator, IBankRoutingParser, IBankAccountValidator, IBankAccountParser
{
    // CLABE check digit weights
    private static readonly int[] Weights = { 3, 7, 1, 3, 7, 1, 3, 7, 1, 3, 7, 1, 3, 7, 1, 3, 7 };

    /// <inheritdoc/>
    public string CountryCode => "MX";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        if (sanitized.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidMexicoClabeLength);
        }

        if (!sanitized.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMexicoClabeFormat);
        }

        // Validate check digit using CLABE algorithm
        if (!ValidateCheckDigit(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidMexicoClabeChecksum);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace(" ", "").Replace("-", "");
        }
        return null;
    }

    /// <inheritdoc/>
    public BankRoutingDetails? ParseRoutingNumber(string? routingNumber)
    {
        var result = Validate(routingNumber);
        if (!result.IsValid || string.IsNullOrEmpty(routingNumber))
        {
            return null;
        }

        var normalized = routingNumber.Replace(" ", "").Replace("-", "");

        // CLABE format: BBB-BBB-AAAAAAAAAAA-C
        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "MX",
            BankCode = normalized.Substring(0, 3),
            BranchCode = normalized.Substring(3, 3),
            CheckDigits = normalized.Substring(17, 1)
        };
    }

    /// <inheritdoc/>
    public BankAccountDetails? ParseBankAccount(string? accountNumber)
    {
        var result = Validate(accountNumber);
        if (!result.IsValid || string.IsNullOrEmpty(accountNumber))
        {
            return null;
        }

        var normalized = accountNumber.Replace(" ", "").Replace("-", "");

        // CLABE format: BBB (bank) + BBB (branch) + AAAAAAAAAAA (account) + C (check)
        return new BankAccountDetails
        {
            AccountNumber = normalized,
            CountryCode = "MX",
            BranchCode = normalized.Substring(3, 3),
            CoreAccountNumber = normalized.Substring(6, 11),
            CheckDigits = normalized.Substring(17, 1)
        };
    }

    /// <summary>
    /// Validates the CLABE check digit using the standard algorithm.
    /// </summary>
    private static bool ValidateCheckDigit(string clabe)
    {
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            int digit = clabe[i] - '0';
            int product = (digit * Weights[i]) % 10;
            sum += product;
        }

        int expectedCheckDigit = (10 - (sum % 10)) % 10;
        int actualCheckDigit = clabe[17] - '0';

        return expectedCheckDigit == actualCheckDigit;
    }
}
