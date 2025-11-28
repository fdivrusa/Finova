using Finova.Core.Models;

namespace Finova.Spain.Models
{
    /// <summary>
    /// Spanish-specific IBAN details.
    /// ES IBAN format: ES + 2 check digits + 20 digit CCC (Código Cuenta Cliente).
    /// CCC Structure: 4 Entidad + 4 Oficina + 2 DC + 10 Cuenta.
    /// Example: ES12 1234 1234 00 1234567890
    /// </summary>
    public record SpainIbanDetails : IbanDetails
    {
        /// <summary>
        /// Entidad (Bank Code).
        /// Identifies the bank (e.g., BBVA, Santander).
        /// Position: 5-8
        /// </summary>
        public required string Entidad { get; init; }

        /// <summary>
        /// Oficina (Branch Code).
        /// Identifies the specific branch office.
        /// Position: 9-12
        /// </summary>
        public required string Oficina { get; init; }

        /// <summary>
        /// Dígito de Control (Control Digits).
        /// 2 digits: 
        /// 1st validates Entidad/Oficina.
        /// 2nd validates Cuenta.
        /// Position: 13-14
        /// </summary>
        public required string DC { get; init; }

        /// <summary>
        /// Número de Cuenta (Account Number).
        /// 10 digits.
        /// Position: 15-24
        /// </summary>
        public required string Cuenta { get; init; }
    }
}