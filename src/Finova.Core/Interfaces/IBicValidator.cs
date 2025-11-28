using Finova.Core.Models;

namespace Finova.Core.Interfaces
{
    public interface IBicValidator
    {
        /// <summary>
        /// Validates the structure of a BIC/SWIFT code.
        /// </summary>
        bool IsValid(string? bic);

        /// <summary>
        /// Parses a BIC into its components (Bank, Country, Location, Branch).
        /// </summary>
        BicDetails? Parse(string? bic);

        /// <summary>
        /// Checks if the BIC's country code matches the provided IBAN country code.
        /// </summary>
        bool IsConsistentWithIban(string? bic, string? ibanCountryCode);
    }
}