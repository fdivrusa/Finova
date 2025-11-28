using Finova.Core.Models;

namespace Finova.Italy.Models
{
    /// <summary>
    /// Italian-specific IBAN details.
    /// IT IBAN format: IT + 2 check digits + 1 CIN + 5 ABI + 5 CAB + 12 account number.
    /// Example: IT60X0542811101000000123456
    /// </summary>
    public record ItalyIbanDetails : IbanDetails
    {
        /// <summary>
        /// Control Internal Number (1 letter).
        /// Position: 5
        /// </summary>
        public required string Cin { get; init; }

        /// <summary>
        /// Associazione Bancaria Italiana (Bank Code - 5 digits).
        /// Position: 6-10
        /// </summary>
        public required string Abi { get; init; }

        /// <summary>
        /// Codice di Avviamento Bancario (Branch Code - 5 digits).
        /// Position: 11-15
        /// </summary>
        public required string Cab { get; init; }

        /// <summary>
        /// Account Number (Numero Conto - 12 alphanumeric characters).
        /// Position: 16-27
        /// </summary>
        public required string NumeroConto { get; init; }
    }
}