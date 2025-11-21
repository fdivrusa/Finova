namespace BankingHelper.Core.Interfaces
{
    public interface IBankAccountValidator
    {
        /// <summary>
        /// ISO country code (ex: "BE", "FR")
        /// </summary>
        string CountryCode { get; }

        bool IsValidIban(string iban);
    }
}
