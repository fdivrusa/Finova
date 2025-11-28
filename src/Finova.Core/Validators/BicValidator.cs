using Finova.Core.Interfaces;
using Finova.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Core.Validators
{
    /// <summary>
    /// Validates and parses Business Identifier Codes (BIC/SWIFT) according to ISO 9362.
    /// Supports both Static usage and Dependency Injection.
    /// </summary>
    public class BicValidator : IBicValidator
    {
        #region Static Methods (High-Performance, Zero-Allocation)

        public static bool IsValid(string? bic)
        {
            if (string.IsNullOrWhiteSpace(bic))
            {
                return false;
            }
            // 8 or 11 characters
            if (bic.Length != 8 && bic.Length != 11)
            {
                return false;
            }

            // 1. Bank Code (4 letters)
            for (int i = 0; i < 4; i++)
            {
                if (!char.IsLetter(bic[i]))
                {
                    return false;
                }
            }

            // 2. Country Code (2 letters)
            for (int i = 4; i < 6; i++)
            {
                if (!char.IsLetter(bic[i]))
                {
                    return false;
                }
            }

            // 3. Location Code (2 alphanumeric)
            for (int i = 6; i < 8; i++)
            {
                if (!char.IsLetterOrDigit(bic[i]))
                {
                    return false;
                }
            }

            // 4. Branch Code (3 alphanumeric - optional)
            if (bic.Length == 11)
            {
                for (int i = 8; i < 11; i++)
                {
                    if (!char.IsLetterOrDigit(bic[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static BicDetails? Parse(string? bic)
        {
            if (!IsValid(bic))
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

        public static bool IsConsistentWithIban(string? bic, string? ibanCountryCode)
        {
            if (string.IsNullOrWhiteSpace(bic) || string.IsNullOrWhiteSpace(ibanCountryCode))
            {
                return false;
            }

            // We only need to check indices 4-6 of the BIC
            if (bic.Length < 6)
            {
                return false;
            }

            // Fast string comparison slice
            return bic.AsSpan(4, 2).Equals(ibanCountryCode.AsSpan(), StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region IBicValidator Implementation (Wrapper for DI)

        // These methods simply delegate to the static logic.
        // This keeps the class stateless and thread-safe.

        bool IBicValidator.IsValid(string? bic) => IsValid(bic);

        BicDetails? IBicValidator.Parse(string? bic) => Parse(bic);

        bool IBicValidator.IsConsistentWithIban(string? bic, string? ibanCountryCode) => IsConsistentWithIban(bic, ibanCountryCode);

        #endregion
    }
}