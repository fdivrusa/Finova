using System.Globalization;
using System.Resources;

namespace Finova.Core.Internals
{
    internal static class ValidationMessages
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("Finova.Core.Internals.ValidationMessages", typeof(ValidationMessages).Assembly);

        public static string VatTooShortForCountryCode => ResourceManager.GetString("VatTooShortForCountryCode", CultureInfo.CurrentUICulture) ?? "VAT number is too short to extract country code.";
        public static string InvalidCountryCodeExpected => ResourceManager.GetString("InvalidCountryCodeExpected", CultureInfo.CurrentUICulture) ?? "Invalid country code. Expected {0}.";
        public static string InvalidUkBankCodeFormat => ResourceManager.GetString("InvalidUkBankCodeFormat", CultureInfo.CurrentUICulture) ?? "UK Bank Code must be letters.";
        public static string InvalidUkSortCodeFormat => ResourceManager.GetString("InvalidUkSortCodeFormat", CultureInfo.CurrentUICulture) ?? "UK Sort Code/Account Number must be digits.";
        public static string InvalidPvmLength => ResourceManager.GetString("InvalidPvmLength", CultureInfo.CurrentUICulture) ?? "PVM must be 9 digits.";
        public static string InvalidUkraineBankCodeFormat => ResourceManager.GetString("InvalidUkraineBankCodeFormat", CultureInfo.CurrentUICulture) ?? "Bank Code (MFO) must be digits.";
        public static string InvalidUkraineAccountNumberFormat => ResourceManager.GetString("InvalidUkraineAccountNumberFormat", CultureInfo.CurrentUICulture) ?? "Account Number must be digits.";
        public static string InvalidSwitzerlandClearingNumberFormat => ResourceManager.GetString("InvalidSwitzerlandClearingNumberFormat", CultureInfo.CurrentUICulture) ?? "Switzerland Clearing Number must be digits.";
        public static string InvalidSwitzerlandAccountNumberFormat => ResourceManager.GetString("InvalidSwitzerlandAccountNumberFormat", CultureInfo.CurrentUICulture) ?? "Switzerland Account Number must be alphanumeric.";
        public static string InvalidIbanDigitsOnly => ResourceManager.GetString("InvalidIbanDigitsOnly", CultureInfo.CurrentUICulture) ?? "{0} IBAN must contain only digits after the country code.";
        public static string InvalidSlovakPrefixChecksum => ResourceManager.GetString("InvalidSlovakPrefixChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Slovak Prefix checksum.";
        public static string InvalidSlovakAccountNumberChecksum => ResourceManager.GetString("InvalidSlovakAccountNumberChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Slovak Account Number checksum.";
        public static string InvalidSpanishDcCheck => ResourceManager.GetString("InvalidSpanishDcCheck", CultureInfo.CurrentUICulture) ?? "Invalid Spanish DC check.";
        public static string InvalidNibKey => ResourceManager.GetString("InvalidNibKey", CultureInfo.CurrentUICulture) ?? "Invalid NIB Key.";
        public static string InvalidIsoReferencePrefix => ResourceManager.GetString("InvalidIsoReferencePrefix", CultureInfo.CurrentUICulture) ?? "Reference must start with 'RF'.";
        public static string InvalidIsoReferenceLength => ResourceManager.GetString("InvalidIsoReferenceLength", CultureInfo.CurrentUICulture) ?? "Reference length is too short.";
        public static string InvalidSwedishOcrLength => ResourceManager.GetString("InvalidSwedishOcrLength", CultureInfo.CurrentUICulture) ?? "Swedish OCR reference must be between 2 and 25 digits.";
        public static string InvalidLengthDigit => ResourceManager.GetString("InvalidLengthDigit", CultureInfo.CurrentUICulture) ?? "Invalid length digit.";
        public static string InvalidFinnishReferenceLength => ResourceManager.GetString("InvalidFinnishReferenceLength", CultureInfo.CurrentUICulture) ?? "Finnish reference must be between 4 and 20 digits.";
        public static string InvalidSwissQrReference => ResourceManager.GetString("InvalidSwissQrReference", CultureInfo.CurrentUICulture) ?? "Invalid Swiss QR-Reference.";
        public static string InvalidSwedishOcrReference => ResourceManager.GetString("InvalidSwedishOcrReference", CultureInfo.CurrentUICulture) ?? "Invalid Swedish OCR reference.";
        public static string InvalidCrnLength => ResourceManager.GetString("InvalidCrnLength", CultureInfo.CurrentUICulture) ?? "CRN must be 8 characters.";
        public static string InvalidCrnFormat => ResourceManager.GetString("InvalidCrnFormat", CultureInfo.CurrentUICulture) ?? "Invalid CRN format.";
        public static string InvalidCifFormat => ResourceManager.GetString("InvalidCifFormat", CultureInfo.CurrentUICulture) ?? "Invalid CIF format.";
        public static string InvalidCifCheckLetter => ResourceManager.GetString("InvalidCifCheckLetter", CultureInfo.CurrentUICulture) ?? "Invalid CIF check letter.";
        public static string InvalidCifCheckDigit => ResourceManager.GetString("InvalidCifCheckDigit", CultureInfo.CurrentUICulture) ?? "Invalid CIF check digit.";
        public static string InvalidCifCheckCharacter => ResourceManager.GetString("InvalidCifCheckCharacter", CultureInfo.CurrentUICulture) ?? "Invalid CIF check character.";
        public static string InvalidIban => ResourceManager.GetString("InvalidIban", CultureInfo.CurrentUICulture) ?? "Invalid IBAN.";
        public static string InvalidAbnLength => ResourceManager.GetString("InvalidAbnLength", CultureInfo.CurrentUICulture) ?? "ABN must be 11 digits.";
        public static string InvalidAbnFormat => ResourceManager.GetString("InvalidAbnFormat", CultureInfo.CurrentUICulture) ?? "ABN must contain only digits.";
        public static string InvalidAbnChecksum => ResourceManager.GetString("InvalidAbnChecksum", CultureInfo.CurrentUICulture) ?? "Invalid ABN checksum.";
        public static string InvalidTfnLength => ResourceManager.GetString("InvalidTfnLength", CultureInfo.CurrentUICulture) ?? "TFN must be 8 or 9 digits.";
        public static string InvalidTfnFormat => ResourceManager.GetString("InvalidTfnFormat", CultureInfo.CurrentUICulture) ?? "TFN must contain only digits.";
        public static string InvalidTfnChecksum => ResourceManager.GetString("InvalidTfnChecksum", CultureInfo.CurrentUICulture) ?? "Invalid TFN checksum.";
        public static string InvalidVaticanVatFormat => ResourceManager.GetString("InvalidVaticanVatFormat", CultureInfo.CurrentUICulture) ?? "Invalid Vatican City VAT format.";
        public static string InvalidVaticanVatChecksum => ResourceManager.GetString("InvalidVaticanVatChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Vatican City VAT checksum.";
        public static string InvalidBnLength => ResourceManager.GetString("InvalidBnLength", CultureInfo.CurrentUICulture) ?? "BN must be 9 digits.";
        public static string InvalidBnFormat => ResourceManager.GetString("InvalidBnFormat", CultureInfo.CurrentUICulture) ?? "BN must contain only digits.";
        public static string InvalidBnChecksum => ResourceManager.GetString("InvalidBnChecksum", CultureInfo.CurrentUICulture) ?? "Invalid BN checksum.";
        public static string InvalidEinLength => ResourceManager.GetString("InvalidEinLength", CultureInfo.CurrentUICulture) ?? "EIN must be 9 digits.";
        public static string InvalidEinFormat => ResourceManager.GetString("InvalidEinFormat", CultureInfo.CurrentUICulture) ?? "EIN must contain only digits.";
        public static string InvalidSinLength => ResourceManager.GetString("InvalidSinLength", CultureInfo.CurrentUICulture) ?? "SIN must be 9 digits.";
        public static string InvalidSinFormat => ResourceManager.GetString("InvalidSinFormat", CultureInfo.CurrentUICulture) ?? "SIN must contain only digits.";
        public static string InvalidSinChecksum => ResourceManager.GetString("InvalidSinChecksum", CultureInfo.CurrentUICulture) ?? "Invalid SIN checksum.";
        public static string InvalidSloveniaVatFormat => ResourceManager.GetString("InvalidSloveniaVatFormat", CultureInfo.CurrentUICulture) ?? "Invalid Slovenia VAT format.";
        public static string InvalidSloveniaVatChecksum => ResourceManager.GetString("InvalidSloveniaVatChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Slovenia VAT checksum.";
        public static string InvalidSloveniaVatChecksumForbidden => ResourceManager.GetString("InvalidSloveniaVatChecksumForbidden", CultureInfo.CurrentUICulture) ?? "Invalid Slovenia VAT checksum (forbidden remainder).";
        public static string InvalidSlovenianSi12Reference => ResourceManager.GetString("InvalidSlovenianSi12Reference", CultureInfo.CurrentUICulture) ?? "Invalid Slovenian SI12 reference.";
        public static string InvalidIbanLengthExpected => ResourceManager.GetString("InvalidIbanLengthExpected", CultureInfo.CurrentUICulture) ?? "Invalid length. Expected {0}, got {1}.";
        public static string InvalidPolandNipFormat => ResourceManager.GetString("InvalidPolandNipFormat", CultureInfo.CurrentUICulture) ?? "Invalid Poland NIP format.";
        public static string InvalidPolandNipChecksumForbidden => ResourceManager.GetString("InvalidPolandNipChecksumForbidden", CultureInfo.CurrentUICulture) ?? "Invalid Poland NIP checksum (forbidden remainder).";
        public static string InvalidPolandNipChecksum => ResourceManager.GetString("InvalidPolandNipChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Poland NIP checksum.";
        public static string InvalidUkraineEdrpouFormat => ResourceManager.GetString("InvalidUkraineEdrpouFormat", CultureInfo.CurrentUICulture) ?? "Invalid Ukraine EDRPOU format.";
        public static string InvalidUkraineEdrpouChecksum => ResourceManager.GetString("InvalidUkraineEdrpouChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Ukraine EDRPOU checksum.";
        public static string InvalidTurkeyVknFormat => ResourceManager.GetString("InvalidTurkeyVknFormat", CultureInfo.CurrentUICulture) ?? "Invalid Turkey VKN format.";
        public static string InvalidTurkeyVknChecksum => ResourceManager.GetString("InvalidTurkeyVknChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Turkey VKN checksum.";
        public static string InvalidSwedenVatFormat => ResourceManager.GetString("InvalidSwedenVatFormat", CultureInfo.CurrentUICulture) ?? "Invalid Sweden VAT format.";
        public static string InvalidSwedenVatChecksum => ResourceManager.GetString("InvalidSwedenVatChecksum", CultureInfo.CurrentUICulture) ?? "Invalid Sweden VAT checksum.";
        public static string InvalidNorthMacedoniaVatFormat => ResourceManager.GetString("InvalidNorthMacedoniaVatFormat", CultureInfo.CurrentUICulture) ?? "Invalid North Macedonia VAT format.";
        public static string InvalidNorthMacedoniaVatChecksumForbidden => ResourceManager.GetString("InvalidNorthMacedoniaVatChecksumForbidden", CultureInfo.CurrentUICulture) ?? "Invalid North Macedonia VAT checksum (Check digit 10).";
        public static string InvalidNorthMacedoniaVatChecksum => ResourceManager.GetString("InvalidNorthMacedoniaVatChecksum", CultureInfo.CurrentUICulture) ?? "Invalid North Macedonia VAT checksum.";
    }
}
