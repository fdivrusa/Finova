using System.Collections.Frozen;
using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Details for a validated currency code.
/// </summary>
public class CurrencyDetails
{
    /// <summary>
    /// The ISO 4217 currency code (3 characters).
    /// </summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// The currency name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The numeric code (3 digits).
    /// </summary>
    public string NumericCode { get; init; } = string.Empty;

    /// <summary>
    /// The number of minor units (decimal places).
    /// </summary>
    public int MinorUnits { get; init; }

    /// <summary>
    /// Whether the currency is valid.
    /// </summary>
    public bool IsValid { get; init; }
}

/// <summary>
/// Interface for Currency code validation.
/// </summary>
public interface ICurrencyValidator
{
    /// <summary>
    /// Validates an ISO 4217 currency code.
    /// </summary>
    ValidationResult Validate(string? currencyCode);

    /// <summary>
    /// Parses a currency code and returns detailed information.
    /// </summary>
    CurrencyDetails? Parse(string? currencyCode);
}

/// <summary>
/// Validator for ISO 4217 currency codes.
/// </summary>
/// <remarks>
/// Validates and provides details for standard currency codes (EUR, USD, GBP, etc.).
/// </remarks>
/// <example>
/// <code>
/// // Validate a currency code
/// var result = CurrencyValidator.Validate("EUR");
/// if (result.IsValid)
/// {
///     Console.WriteLine("Valid currency");
/// }
/// 
/// // Parse a currency code
/// var details = CurrencyValidator.Parse("USD");
/// Console.WriteLine($"Name: {details?.Name}"); // US Dollar
/// Console.WriteLine($"Minor Units: {details?.MinorUnits}"); // 2
/// </code>
/// </example>
public class CurrencyValidator : ICurrencyValidator
{
    // ISO 4217 currency data: Code -> (Name, NumericCode, MinorUnits)
    private static readonly FrozenDictionary<string, (string Name, string NumericCode, int MinorUnits)> Currencies = new Dictionary<string, (string, string, int)>
    {
        // Major currencies
        ["EUR"] = ("Euro", "978", 2),
        ["USD"] = ("US Dollar", "840", 2),
        ["GBP"] = ("Pound Sterling", "826", 2),
        ["JPY"] = ("Yen", "392", 0),
        ["CHF"] = ("Swiss Franc", "756", 2),
        ["AUD"] = ("Australian Dollar", "036", 2),
        ["CAD"] = ("Canadian Dollar", "124", 2),
        ["CNY"] = ("Yuan Renminbi", "156", 2),
        ["HKD"] = ("Hong Kong Dollar", "344", 2),
        ["NZD"] = ("New Zealand Dollar", "554", 2),
        ["SEK"] = ("Swedish Krona", "752", 2),
        ["KRW"] = ("Won", "410", 0),
        ["SGD"] = ("Singapore Dollar", "702", 2),
        ["NOK"] = ("Norwegian Krone", "578", 2),
        ["MXN"] = ("Mexican Peso", "484", 2),
        ["INR"] = ("Indian Rupee", "356", 2),
        ["RUB"] = ("Russian Ruble", "643", 2),
        ["ZAR"] = ("Rand", "710", 2),
        ["TRY"] = ("Turkish Lira", "949", 2),
        ["BRL"] = ("Brazilian Real", "986", 2),
        
        // European currencies
        ["DKK"] = ("Danish Krone", "208", 2),
        ["PLN"] = ("Zloty", "985", 2),
        ["CZK"] = ("Czech Koruna", "203", 2),
        ["HUF"] = ("Forint", "348", 2),
        ["RON"] = ("Romanian Leu", "946", 2),
        ["BGN"] = ("Bulgarian Lev", "975", 2),
        ["HRK"] = ("Kuna", "191", 2),
        ["ISK"] = ("Iceland Krona", "352", 0),
        ["RSD"] = ("Serbian Dinar", "941", 2),
        ["UAH"] = ("Hryvnia", "980", 2),
        ["ALL"] = ("Lek", "008", 2),
        ["MKD"] = ("Denar", "807", 2),
        ["BAM"] = ("Convertible Mark", "977", 2),
        ["MDL"] = ("Moldovan Leu", "498", 2),
        ["GEL"] = ("Lari", "981", 2),
        ["AMD"] = ("Armenian Dram", "051", 2),
        ["AZN"] = ("Azerbaijan Manat", "944", 2),
        ["BYN"] = ("Belarusian Ruble", "933", 2),
        
        // Middle East currencies
        ["AED"] = ("UAE Dirham", "784", 2),
        ["SAR"] = ("Saudi Riyal", "682", 2),
        ["ILS"] = ("New Israeli Sheqel", "376", 2),
        ["QAR"] = ("Qatari Rial", "634", 2),
        ["KWD"] = ("Kuwaiti Dinar", "414", 3),
        ["BHD"] = ("Bahraini Dinar", "048", 3),
        ["OMR"] = ("Rial Omani", "512", 3),
        ["JOD"] = ("Jordanian Dinar", "400", 3),
        ["LBP"] = ("Lebanese Pound", "422", 2),
        ["EGP"] = ("Egyptian Pound", "818", 2),
        
        // Asian currencies
        ["THB"] = ("Baht", "764", 2),
        ["MYR"] = ("Malaysian Ringgit", "458", 2),
        ["IDR"] = ("Rupiah", "360", 2),
        ["PHP"] = ("Philippine Peso", "608", 2),
        ["VND"] = ("Dong", "704", 0),
        ["TWD"] = ("New Taiwan Dollar", "901", 2),
        ["PKR"] = ("Pakistan Rupee", "586", 2),
        ["BDT"] = ("Taka", "050", 2),
        ["LKR"] = ("Sri Lanka Rupee", "144", 2),
        ["NPR"] = ("Nepalese Rupee", "524", 2),
        ["MMK"] = ("Kyat", "104", 2),
        ["KHR"] = ("Riel", "116", 2),
        ["LAK"] = ("Lao Kip", "418", 2),
        ["MNT"] = ("Tugrik", "496", 2),
        ["KZT"] = ("Tenge", "398", 2),
        ["UZS"] = ("Uzbekistan Sum", "860", 2),
        
        // African currencies
        ["NGN"] = ("Naira", "566", 2),
        ["KES"] = ("Kenyan Shilling", "404", 2),
        ["GHS"] = ("Ghana Cedi", "936", 2),
        ["TZS"] = ("Tanzanian Shilling", "834", 2),
        ["UGX"] = ("Uganda Shilling", "800", 0),
        ["MAD"] = ("Moroccan Dirham", "504", 2),
        ["DZD"] = ("Algerian Dinar", "012", 2),
        ["TND"] = ("Tunisian Dinar", "788", 3),
        ["XOF"] = ("CFA Franc BCEAO", "952", 0),
        ["XAF"] = ("CFA Franc BEAC", "950", 0),
        
        // Americas currencies
        ["ARS"] = ("Argentine Peso", "032", 2),
        ["CLP"] = ("Chilean Peso", "152", 0),
        ["COP"] = ("Colombian Peso", "170", 2),
        ["PEN"] = ("Sol", "604", 2),
        ["VES"] = ("Bolívar Soberano", "928", 2),
        ["UYU"] = ("Peso Uruguayo", "858", 2),
        ["PYG"] = ("Guarani", "600", 0),
        ["BOB"] = ("Boliviano", "068", 2),
        ["DOP"] = ("Dominican Peso", "214", 2),
        ["CRC"] = ("Costa Rican Colon", "188", 2),
        ["GTQ"] = ("Quetzal", "320", 2),
        ["HNL"] = ("Lempira", "340", 2),
        ["NIO"] = ("Cordoba Oro", "558", 2),
        ["PAB"] = ("Balboa", "590", 2),
        ["JMD"] = ("Jamaican Dollar", "388", 2),
        ["TTD"] = ("Trinidad and Tobago Dollar", "780", 2),
        
        // Oceania currencies
        ["FJD"] = ("Fiji Dollar", "242", 2),
        ["PGK"] = ("Kina", "598", 2),
        ["WST"] = ("Tala", "882", 2),
        ["VUV"] = ("Vatu", "548", 0),
        ["SBD"] = ("Solomon Islands Dollar", "090", 2),
        ["TOP"] = ("Pa'anga", "776", 2),
        
        // Precious metals (per troy ounce)
        ["XAU"] = ("Gold", "959", -1),
        ["XAG"] = ("Silver", "961", -1),
        ["XPT"] = ("Platinum", "962", -1),
        ["XPD"] = ("Palladium", "964", -1),
        
        // Special currencies
        ["XDR"] = ("SDR (Special Drawing Right)", "960", -1),
        ["XXX"] = ("No currency", "999", -1),
        ["XTS"] = ("Testing", "963", -1),
        
        // Cryptocurrency placeholders (not official ISO 4217 but commonly used)
        ["BTC"] = ("Bitcoin", "---", 8),
        ["ETH"] = ("Ether", "---", 18),
    }.ToFrozenDictionary();

    /// <summary>
    /// Validates an ISO 4217 currency code.
    /// </summary>
    /// <param name="currencyCode">The currency code to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult Validate(string? currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(currencyCode);

        if (normalized.Length != 3)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Currency code must be exactly 3 characters.");
        }

        if (!Currencies.ContainsKey(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Unknown currency code: {normalized}");
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Parses a currency code and returns detailed information.
    /// </summary>
    /// <param name="currencyCode">The currency code to parse.</param>
    /// <returns>A <see cref="CurrencyDetails"/> object if valid, otherwise null.</returns>
    public static CurrencyDetails? Parse(string? currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return null;
        }

        var normalized = Normalize(currencyCode);

        if (normalized.Length != 3 || !Currencies.TryGetValue(normalized, out var data))
        {
            return null;
        }

        return new CurrencyDetails
        {
            Code = normalized,
            Name = data.Name,
            NumericCode = data.NumericCode,
            MinorUnits = data.MinorUnits,
            IsValid = true
        };
    }

    /// <summary>
    /// Checks if a currency code is valid.
    /// </summary>
    /// <param name="currencyCode">The currency code to check.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool IsValid(string? currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return false;
        }

        var normalized = Normalize(currencyCode);
        return normalized.Length == 3 && Currencies.ContainsKey(normalized);
    }

    /// <summary>
    /// Gets a list of all supported currency codes.
    /// </summary>
    /// <returns>An enumerable of all supported currency codes.</returns>
    public static IEnumerable<string> GetAllCurrencyCodes() => Currencies.Keys;

    private static string Normalize(string currencyCode)
    {
        return currencyCode.Trim().ToUpperInvariant();
    }

    // ICurrencyValidator implementation
    ValidationResult ICurrencyValidator.Validate(string? currencyCode) => Validate(currencyCode);
    CurrencyDetails? ICurrencyValidator.Parse(string? currencyCode) => Parse(currencyCode);
}
