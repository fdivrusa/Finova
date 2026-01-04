using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Argentina.Validators;

/// <summary>
/// Validates Argentine CBU (Clave Bancaria Uniforme).
/// Format: 22 digits - EEEE-SSSS-C-AAAAAAAAAAAAA-D
/// Where: EEEE = Bank code (4 digits, including 3-digit bank + 1 reserved),
/// SSSS = Branch code (4 digits), C = First check digit,
/// AAAAAAAAAAAAA = Account number (13 digits), D = Second check digit.
/// </summary>
public class ArgentinaCbuValidator : IBankRoutingValidator, IBankRoutingParser, IBankAccountValidator, IBankAccountParser
{
    /// <inheritdoc/>
    public string CountryCode => "AR";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "").Replace("-", "");

        if (sanitized.Length != 22)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidArgentinaCbuLength);
        }

        if (!sanitized.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidArgentinaCbuFormat);
        }

        // Validate both check digits
        if (!ValidateCheckDigits(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidArgentinaCbuChecksum);
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

        // CBU format: Bank (3) + Reserved (1) + Branch (3) + Check1 (1) + Account (13) + Check2 (1)
        return new BankRoutingDetails
        {
            RoutingNumber = normalized,
            CountryCode = "AR",
            BankCode = normalized.Substring(0, 3),
            BranchCode = normalized.Substring(4, 3),
            CheckDigits = normalized[7].ToString() + normalized[21]
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

        // CBU format: Bank (3) + Reserved (1) + Branch (3) + Check1 (1) + Account (13) + Check2 (1)
        return new BankAccountDetails
        {
            AccountNumber = normalized,
            CountryCode = "AR",
            BranchCode = normalized.Substring(4, 3),
            CoreAccountNumber = normalized.Substring(8, 13),
            CheckDigits = normalized[7].ToString() + normalized[21]
        };
    }

    /// <summary>
    /// Validates both CBU check digits using the standard algorithm.
    /// </summary>
    private static bool ValidateCheckDigits(string cbu)
    {
        // First check digit (position 8) validates first 7 digits
        // Weights: 7, 1, 3, 9, 7, 1, 3
        int[] weights1 = { 7, 1, 3, 9, 7, 1, 3 };
        int sum1 = 0;
        for (int i = 0; i < 7; i++)
        {
            sum1 += (cbu[i] - '0') * weights1[i];
        }
        int checkDigit1 = (10 - (sum1 % 10)) % 10;
        if (checkDigit1 != cbu[7] - '0')
        {
            return false;
        }

        // Second check digit (position 22) validates digits 9-21
        // Weights: 3, 9, 7, 1, 3, 9, 7, 1, 3, 9, 7, 1, 3
        int[] weights2 = { 3, 9, 7, 1, 3, 9, 7, 1, 3, 9, 7, 1, 3 };
        int sum2 = 0;
        for (int i = 0; i < 13; i++)
        {
            sum2 += (cbu[8 + i] - '0') * weights2[i];
        }
        int checkDigit2 = (10 - (sum2 % 10)) % 10;
        if (checkDigit2 != cbu[21] - '0')
        {
            return false;
        }

        return true;
    }
}
