using Finova.Core.Models;

namespace Finova.Core.Interfaces
{
    public interface IIbanValidator
    {
        /// <summary>
        /// ISO country code (ex: "BE", "FR")
        /// </summary>
        string CountryCode { get; }

        /// <summary>
        /// Validates an IBAN for the specific country.
        /// </summary>
        bool IsValidIban(string iban);
    }
}
