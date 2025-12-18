using Finova.Core.Enterprise;
using Finova.Core.Vat;
using Finova.Core.PaymentReference;

namespace Finova.Services;

/// <summary>
/// Defines the scope of an identifier (VAT, Enterprise, or both).
/// </summary>
[Flags]
public enum IdentifierScope
{
    None = 0,
    Vat = 1,
    Enterprise = 2,
    Both = Vat | Enterprise
}

/// <summary>
/// Defines the semantic kind of an identifier.
/// </summary>
public enum IdentifierKind
{
    Vat,
    BusinessTaxId,
    InvoicingSchemeIdentifier,
    NotApplicable
}

/// <summary>
/// Centralized metadata for European country identifiers.
/// </summary>
public static class EuropeCountryIdentifierMetadata
{
    public record CountryMetadata(
        string CountryCode,
        IdentifierScope SupportedScopes,
        IdentifierKind VatKind,
        bool IsEuVat,
        bool IsViesEligible,
        string? Note = null,
        EnterpriseNumberType? DefaultEnterpriseType = null,
        string? EnterpriseNumberEnglishName = null,
        string? EnterpriseNumberLocalName = null,
        PaymentReferenceFormat? LocalPaymentReferenceFormat = null,
        string? PaymentReferenceLocalName = null
    );

    private static readonly Dictionary<string, CountryMetadata> Metadata = new(StringComparer.OrdinalIgnoreCase)
    {
        // EU Countries (VIES Eligible)
        { "AT", new("AT", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.AustriaFirmenbuch, EnterpriseNumberEnglishName: "Company Register Number", EnterpriseNumberLocalName: "Firmenbuchnummer") },
        { "BE", new("BE", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.BelgiumEnterpriseNumber, EnterpriseNumberEnglishName: "Enterprise Number", EnterpriseNumberLocalName: "Ondernemingsnummer / Numéro d'entreprise", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalBelgian, PaymentReferenceLocalName: "OGM / VCS") },
        { "BG", new("BG", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.BulgariaUic, EnterpriseNumberEnglishName: "Unified Identity Code", EnterpriseNumberLocalName: "ЕИК (EIK)") },
        { "CY", new("CY", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.CyprusTic, EnterpriseNumberEnglishName: "Tax Identification Code", EnterpriseNumberLocalName: "Αριθμός Φορολογικής Ταυτότητας") },
        { "CZ", new("CZ", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.CzechRepublicIco, EnterpriseNumberEnglishName: "Identification Number", EnterpriseNumberLocalName: "IČO (Identifikační číslo osoby)") },
        { "DE", new("DE", IdentifierScope.Both, IdentifierKind.Vat, true, true, EnterpriseNumberEnglishName: "Tax Number / Commercial Register Number", EnterpriseNumberLocalName: "Steuernummer / Handelsregisternummer") }, // Germany (Complex dispatch)
        { "DK", new("DK", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.DenmarkCvr, EnterpriseNumberEnglishName: "Central Business Register Number", EnterpriseNumberLocalName: "CVR-nummer", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalDenmark, PaymentReferenceLocalName: "FIK / GIK") },
        { "EE", new("EE", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.EstoniaRegistrikood, EnterpriseNumberEnglishName: "Commercial Register Code", EnterpriseNumberLocalName: "Registrikood") },
        { "EL", new("EL", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.GreeceAfm, EnterpriseNumberEnglishName: "Tax Identification Number", EnterpriseNumberLocalName: "ΑΦΜ (Arithmos Forologikou Mitroou)") }, // Greece
        { "GR", new("GR", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.GreeceAfm, EnterpriseNumberEnglishName: "Tax Identification Number", EnterpriseNumberLocalName: "ΑΦΜ (Arithmos Forologikou Mitroou)") }, // Greece (Alt)
        { "ES", new("ES", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.SpainCif, EnterpriseNumberEnglishName: "Tax Identification Number", EnterpriseNumberLocalName: "NIF (Número de Identificación Fiscal)") }, // Spain
        { "FI", new("FI", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.FinlandBusinessId, EnterpriseNumberEnglishName: "Business ID", EnterpriseNumberLocalName: "Y-tunnus", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalFinland, PaymentReferenceLocalName: "Viitenumero") },
        { "FR", new("FR", IdentifierScope.Both, IdentifierKind.Vat, true, true, EnterpriseNumberEnglishName: "Business Directory ID", EnterpriseNumberLocalName: "SIREN / SIRET") }, // France (Complex dispatch SIREN/SIRET)
        { "HR", new("HR", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.CroatiaOib, EnterpriseNumberEnglishName: "Personal Identification Number", EnterpriseNumberLocalName: "OIB (Osobni identifikacijski broj)") },
        { "HU", new("HU", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.HungaryAdoszam, EnterpriseNumberEnglishName: "Tax Number", EnterpriseNumberLocalName: "Adószám") }, // Hungary
        { "IE", new("IE", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.IrelandVat, EnterpriseNumberEnglishName: "VAT/Tax Reference Number", EnterpriseNumberLocalName: "Cáin Bhreisluacha (CBL)") }, // Enterprise uses VAT
        { "IT", new("IT", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.ItalyCodiceFiscale, EnterpriseNumberEnglishName: "Tax Code", EnterpriseNumberLocalName: "Codice Fiscale", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalItaly, PaymentReferenceLocalName: "CBILL / PagoPA / IUV") },
        { "LT", new("LT", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.LithuaniaPvm, EnterpriseNumberEnglishName: "VAT Code", EnterpriseNumberLocalName: "PVM kodas") },
        { "LU", new("LU", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.LuxembourgTva, EnterpriseNumberEnglishName: "VAT Number", EnterpriseNumberLocalName: "Numéro d'identification TVA") }, // Enterprise uses VAT
        { "LV", new("LV", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.LatviaPvn, EnterpriseNumberEnglishName: "VAT Number", EnterpriseNumberLocalName: "PVN (Pievienotās vērtības nodoklis)") },
        { "MT", new("MT", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.MaltaVat, EnterpriseNumberEnglishName: "VAT Number", EnterpriseNumberLocalName: "Vat Number") }, // Enterprise uses VAT
        { "NL", new("NL", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.NetherlandsBtw, EnterpriseNumberEnglishName: "VAT Number / RSIN", EnterpriseNumberLocalName: "Btw-nummer / RSIN") }, // Enterprise uses VAT
        { "PL", new("PL", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.PolandNip, EnterpriseNumberEnglishName: "Tax Identification Number", EnterpriseNumberLocalName: "NIP (Numer Identyfikacji Podatkowej)") },
        { "PT", new("PT", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.PortugalNif, EnterpriseNumberEnglishName: "Tax Identification Number", EnterpriseNumberLocalName: "NIF (Número de Identificação Fiscal)", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalPortugal, PaymentReferenceLocalName: "Multibanco Reference") },
        { "RO", new("RO", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.RomaniaCif, EnterpriseNumberEnglishName: "Fiscal Identification Code", EnterpriseNumberLocalName: "CIF (Cod de Identificare Fiscală)") },
        { "SE", new("SE", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.SwedenMoms, EnterpriseNumberEnglishName: "VAT Number", EnterpriseNumberLocalName: "Momsnummer", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalSweden, PaymentReferenceLocalName: "OCR / Bankgiro") },
        { "SI", new("SI", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.SloveniaTaxNumber, EnterpriseNumberEnglishName: "Tax Number", EnterpriseNumberLocalName: "Davčna številka", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalSlovenia, PaymentReferenceLocalName: "Sklic") },
        { "SK", new("SK", IdentifierScope.Both, IdentifierKind.Vat, true, true, DefaultEnterpriseType: EnterpriseNumberType.SlovakiaVat, EnterpriseNumberEnglishName: "VAT Number", EnterpriseNumberLocalName: "IČ DPH") },

        // Non-EU VAT Countries (Not VIES)
        { "IS", new("IS", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.IcelandKennitala, EnterpriseNumberEnglishName: "ID Number", EnterpriseNumberLocalName: "Kennitala") },
        { "NO", new("NO", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.NorwayOrgNumber, EnterpriseNumberEnglishName: "Organization Number", EnterpriseNumberLocalName: "Organisasjonsnummer", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalNorway, PaymentReferenceLocalName: "KID (Kundeidentifikasjonsnummer)") },
        { "FO", new("FO", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.FaroeIslandsVtal, EnterpriseNumberEnglishName: "Tax Number", EnterpriseNumberLocalName: "V-tal") },
        { "CH", new("CH", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.SwitzerlandUid, EnterpriseNumberEnglishName: "Business ID", EnterpriseNumberLocalName: "UID (Unternehmens-Identifikationsnummer)", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalSwitzerland, PaymentReferenceLocalName: "QR-Reference") },
        { "CHE", new("CHE", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.SwitzerlandUid, EnterpriseNumberEnglishName: "Business ID", EnterpriseNumberLocalName: "UID", LocalPaymentReferenceFormat: PaymentReferenceFormat.LocalSwitzerland, PaymentReferenceLocalName: "QR-Reference") },
        { "GB", new("GB", IdentifierScope.Both, IdentifierKind.Vat, false, false, "Post-Brexit UK VAT", DefaultEnterpriseType: EnterpriseNumberType.UnitedKingdomCompanyNumber, EnterpriseNumberEnglishName: "Company Number", EnterpriseNumberLocalName: "Company Number") },

        // Business Tax IDs / Invoicing Identifiers
        { "TR", new("TR", IdentifierScope.Both, IdentifierKind.BusinessTaxId, false, false, "VKN is a tax ID, not EU VAT", DefaultEnterpriseType: EnterpriseNumberType.TurkeyVkn, EnterpriseNumberEnglishName: "Tax Identification Number", EnterpriseNumberLocalName: "VKN (Vergi Kimlik Numarası)") },
        { "GE", new("GE", IdentifierScope.Both, IdentifierKind.BusinessTaxId, false, false, "Tax ID", DefaultEnterpriseType: EnterpriseNumberType.GeorgiaTaxId, EnterpriseNumberEnglishName: "Tax ID", EnterpriseNumberLocalName: " საიდენტიფიკაციო კოდი") },
        { "AZ", new("AZ", IdentifierScope.Both, IdentifierKind.BusinessTaxId, false, false, "VÖEN is a tax ID", DefaultEnterpriseType: EnterpriseNumberType.AzerbaijanVoen, EnterpriseNumberEnglishName: "Taxpayer ID", EnterpriseNumberLocalName: "VÖEN") },
        { "UA", new("UA", IdentifierScope.Both, IdentifierKind.BusinessTaxId, false, false, "EDRPOU is a registry number", DefaultEnterpriseType: EnterpriseNumberType.UkraineEdrpou, EnterpriseNumberEnglishName: "Unified State Register Code", EnterpriseNumberLocalName: "ЄДРПОУ (EDRPOU)") },
        { "SM", new("SM", IdentifierScope.Both, IdentifierKind.BusinessTaxId, false, false, "COE is a tax ID", DefaultEnterpriseType: EnterpriseNumberType.SanMarinoCoe, EnterpriseNumberEnglishName: "Economic Operator Code", EnterpriseNumberLocalName: "COE (Codice Operatore Economico)") },
        
        // Vatican (Invoicing Scheme)
        { "VA", new("VA", IdentifierScope.Both, IdentifierKind.InvoicingSchemeIdentifier, false, false, "Use for e-invoicing participant ID; not a legal VAT registration number", DefaultEnterpriseType: EnterpriseNumberType.VaticanCityVat, EnterpriseNumberEnglishName: "Vatican ID", EnterpriseNumberLocalName: "N/A") },

        // Others
        { "AL", new("AL", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.AlbaniaNipt, EnterpriseNumberEnglishName: "Tax ID", EnterpriseNumberLocalName: "NIPT") },
        { "AD", new("AD", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.AndorraNrt, EnterpriseNumberEnglishName: "Tax Register Number", EnterpriseNumberLocalName: "NRT (Número de Registre Tributari)") },
        { "BA", new("BA", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.BosniaJib, EnterpriseNumberEnglishName: "Unique ID Number", EnterpriseNumberLocalName: "JIB (Jedinstveni identifikacioni broj)") },
        { "BY", new("BY", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.BelarusUnp, EnterpriseNumberEnglishName: "Payer ID", EnterpriseNumberLocalName: "УНП (UNP)") },
        { "GI", new("GI", IdentifierScope.Enterprise, IdentifierKind.BusinessTaxId, false, false, DefaultEnterpriseType: EnterpriseNumberType.GibraltarCompanyNumber, EnterpriseNumberEnglishName: "Company Number", EnterpriseNumberLocalName: "Company Number") },
        { "GL", new("GL", IdentifierScope.Enterprise, IdentifierKind.BusinessTaxId, false, false, DefaultEnterpriseType: EnterpriseNumberType.GreenlandCvr, EnterpriseNumberEnglishName: "Central Business Register", EnterpriseNumberLocalName: "CVR") },
        { "LI", new("LI", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.LiechtensteinPeid, EnterpriseNumberEnglishName: "Person ID", EnterpriseNumberLocalName: "PEID") },
        { "MC", new("MC", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.MonacoRci, EnterpriseNumberEnglishName: "Trade & Industry Register", EnterpriseNumberLocalName: "RCI (Répertoire du Commerce et de l'Industrie)") }, // Monaco
        { "MD", new("MD", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.MoldovaIdno, EnterpriseNumberEnglishName: "State ID Number", EnterpriseNumberLocalName: "IDNO") },
        { "ME", new("ME", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.MontenegroPib, EnterpriseNumberEnglishName: "Tax ID", EnterpriseNumberLocalName: "PIB") },
        { "MK", new("MK", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.NorthMacedoniaEdb, EnterpriseNumberEnglishName: "Unique Tax Number", EnterpriseNumberLocalName: "EDB") },
        { "RS", new("RS", IdentifierScope.Both, IdentifierKind.Vat, false, false, DefaultEnterpriseType: EnterpriseNumberType.SerbiaPib, EnterpriseNumberEnglishName: "Tax ID", EnterpriseNumberLocalName: "PIB") },
        { "XK", new("XK", IdentifierScope.Enterprise, IdentifierKind.BusinessTaxId, false, false, DefaultEnterpriseType: EnterpriseNumberType.KosovoFiscalNumber, EnterpriseNumberEnglishName: "Fiscal Number", EnterpriseNumberLocalName: "Numri Fiskal") },
    };

    public static CountryMetadata? For(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode)) return null;
        return Metadata.TryGetValue(countryCode, out var metadata) ? metadata : null;
    }
}
