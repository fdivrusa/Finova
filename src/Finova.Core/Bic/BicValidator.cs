using Finova.Core.Common;

namespace Finova.Core.Bic;

/// <summary>
/// Validates and parses Business Identifier Codes (BIC/SWIFT) according to ISO 9362.
/// Supports both Static usage and Dependency Injection.
/// </summary>
public class BicValidator : IBicValidator
{
    #region Static Methods (High-Performance, Zero-Allocation)

    public static ValidationResult Validate(string? bic)
    {
        if (string.IsNullOrWhiteSpace(bic))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        // 8 or 11 characters
        if (bic.Length != 8 && bic.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBicLength);
        }

        // 1. Bank Code (4 letters)
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bic[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBicBankCode);
            }
        }

        // 2. Country Code (2 letters)
        for (int i = 4; i < 6; i++)
        {
            if (!char.IsLetter(bic[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBicCountryCode);
            }
        }

        // 3. Location Code (2 alphanumeric)
        for (int i = 6; i < 8; i++)
        {
            if (!char.IsLetterOrDigit(bic[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBicLocationCode);
            }
        }

        // 4. Branch Code (3 alphanumeric - optional)
        if (bic.Length == 11)
        {
            for (int i = 8; i < 11; i++)
            {
                if (!char.IsLetterOrDigit(bic[i]))
                {
                    return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBicBranchCode);
                }
            }
        }

        return ValidationResult.Success();
    }

    public static BicDetails? Parse(string? bic)
    {
        var result = Validate(bic);
        if (!result.IsValid)
        {
            return null;
        }

        string normalized = bic!.ToUpperInvariant();
        string branchCode = normalized.Length == 8 ? "XXX" : normalized[8..11];

        // Normalize to 11 chars for consistency in the model
        if (normalized.Length == 8)
        {
            normalized += "XXX";
        }

        return new BicDetails
        {
            Bic = normalized,
            BankCode = normalized[0..4],
            CountryCode = normalized[4..6],
            LocationCode = normalized[6..8],
            BranchCode = branchCode,
            IsValid = true
        };
    }

    public static ValidationResult ValidateConsistencyWithIban(string? bic, string? ibanCountryCode)
    {
        if (string.IsNullOrWhiteSpace(bic) || string.IsNullOrWhiteSpace(ibanCountryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // We only need to check indices 4-6 of the BIC
        if (bic.Length < 6)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Fast string comparison slice
        bool isConsistent = bic.AsSpan(4, 2).Equals(ibanCountryCode.AsSpan(), StringComparison.OrdinalIgnoreCase);

        return isConsistent
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"{ValidationMessages.InvalidFormat} BIC country code does not match IBAN country code.");
    }

    public static ValidationResult ValidateCompatibilityWithIban(string? bic, string? iban)
    {
        if (string.IsNullOrWhiteSpace(bic) || string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalizedIban = iban.Trim();
        if (normalizedIban.Length < 2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"{ValidationMessages.InvalidFormat} Invalid IBAN length.");
        }

        var countryCode = normalizedIban.Substring(0, 2);
        return ValidateConsistencyWithIban(bic, countryCode);
    }

    #endregion

    #region IBicValidator Implementation (Wrapper for DI)

    ValidationResult IValidator<BicDetails>.Validate(string? input)
    {
        return Validate(input);
    }

    BicDetails? IValidator<BicDetails>.Parse(string? input) => Parse(input);

    ValidationResult IBicValidator.ValidateConsistencyWithIban(string? bic, string? ibanCountryCode) => ValidateConsistencyWithIban(bic, ibanCountryCode);
    ValidationResult IBicValidator.ValidateCompatibilityWithIban(string? bic, string? iban) => ValidateCompatibilityWithIban(bic, iban);

    #endregion
}
