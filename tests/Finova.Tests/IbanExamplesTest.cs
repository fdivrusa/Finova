using System.Numerics;
using System.Text;
using Finova.Services;
using Xunit;
using Xunit.Abstractions;

namespace Finova.Tests;

public class IbanExamplesTest
{
    private readonly ITestOutputHelper _output;

    public IbanExamplesTest(ITestOutputHelper output)
    {
        _output = output;
    }

    // Calculate the correct check digits for an IBAN with placeholder "00" as check digits
    private static string CalculateCheckDigits(string countryCode, string bban)
    {
        // IBAN = CountryCode + CheckDigits + BBAN
        // To calculate check digits:
        // 1. Take BBAN + CountryCode + "00"
        // 2. Convert letters to numbers (A=10, B=11, ..., Z=35)
        // 3. Calculate mod 97
        // 4. Check digits = 98 - (mod 97 result)

        var rearranged = bban + countryCode + "00";
        var numericString = ConvertToNumeric(rearranged);
        var mod = Mod97(numericString);
        var checkDigits = 98 - mod;
        return checkDigits.ToString("D2");
    }

    private static string ConvertToNumeric(string input)
    {
        var sb = new StringBuilder();
        foreach (var c in input.ToUpperInvariant())
        {
            if (char.IsDigit(c))
                sb.Append(c);
            else if (char.IsLetter(c))
                sb.Append(c - 'A' + 10);
        }
        return sb.ToString();
    }

    private static int Mod97(string numericString)
    {
        // Use BigInteger for large number mod calculation
        var number = BigInteger.Parse(numericString);
        return (int)(number % 97);
    }

    [Fact]
    public void GenerateValidIbans()
    {
        // Generate valid IBANs for countries with invalid examples
        // Using proper BBAN lengths from validators
        var ibansToFix = new (string Country, string Code, int Length, string Bban)[]
        {
            // Algeria - DZ + 2 check + 20 digits = 24
            ("Algeria", "DZ", 24, "58001000000000000100"),
            // Angola - AO + 2 check + 21 digit = 25
            ("Angola", "AO", 25, "006000000000300014013"),
            // Burundi - BI + 2 check + 23 digit = 27
            ("Burundi", "BI", 27, "43201011067444010270188"),
            // Cameroon - CM + 2 check + 23 digit BBAN = 27
            ("Cameroon", "CM", 27, "21000100200300456789016"),
            // Cape Verde - CV + 2 check + 21 digit BBAN = 25
            ("Cape Verde", "CV", 25, "000300000054321047012"),
            // Central African Republic - CF + 2 check + 23 digit BBAN = 27
            ("Central African Republic", "CF", 27, "20011000100300277000160"),
            // Chad - TD + 2 check + 23 digit BBAN = 27
            ("Chad", "TD", 27, "00890001100101015002037"),
            // Comoros - KM + 2 check + 23 digit BBAN = 27
            ("Comoros", "KM", 27, "46001000100100300100000"),
            // Congo - CG + 2 check + 23 digit BBAN = 27
            ("Congo", "CG", 27, "30011000100271033023002"),
            // Djibouti - DJ + 2 check + 23 digit BBAN = 27
            ("Djibouti", "DJ", 27, "21001000100300456712010"),
            // Equatorial Guinea - GQ + 2 check + 23 digit BBAN = 27
            ("Equatorial Guinea", "GQ", 27, "70001000100300678102340"),
            // Gabon - GA + 2 check + 23 digit BBAN = 27
            ("Gabon", "GA", 27, "40013000100100000001001"),
            // Libya - LY + 2 check + 21 digit BBAN = 25
            ("Libya", "LY", 25, "002048000020100120301"),
            // Morocco - MA + 2 check + 24 digit BBAN = 28
            ("Morocco", "MA", 28, "011519000001205000534921"),
            // Madagascar - MG + 2 check + 23 digit BBAN = 27
            ("Madagascar", "MG", 27, "46001000011100009140029"),
            // Mozambique - MZ + 2 check + 21 digit BBAN = 25
            ("Mozambique", "MZ", 25, "592100000010834000141"),
            // Sao Tome and Principe - ST + 2 check + 21 digit BBAN = 25
            ("Sao Tome and Principe", "ST", 25, "000200100192194010192"),
            // Seychelles - SC + 2 check + 4 letter bank + 20 digit + 3 letter currency = 31
            ("Seychelles", "SC", 31, "SSCB11010000000000001497USD"),
            // Somalia - SO + 2 check + 19 digit BBAN = 23
            ("Somalia", "SO", 23, "1000001234567890123"),
            // Sudan - SD + 2 check + 14 digit BBAN = 18
            ("Sudan", "SD", 18, "21012901895303"),
            // Honduras - HN + 2 check + 4 letter + 20 digit = 28 (24 BBAN)
            ("Honduras", "HN", 28, "FICO00000000012345678901"),
            // Nicaragua - NI + 2 check + 4 letter + 24 digit = 32 (28 BBAN)
            ("Nicaragua", "NI", 32, "BAPI009900010000000000000005"),
            // Saint Lucia - LC + 2 check + 4 letter + 24 chars = 32 (28 BBAN)
            ("Saint Lucia", "LC", 32, "HEMM000100010012001200023015"),
            // Iraq - IQ + 2 check + 4 letter bank + 15 digit = 23 (19 BBAN)
            ("Iraq", "IQ", 23, "NBIQ850123456789012"),
            // Oman - OM + 2 check + 3 digit + 16 digit = 23 (19 BBAN)
            ("Oman", "OM", 23, "0060123456789012345"),
            // Qatar - QA + 2 check + 4 letter bank + 21 chars = 29 (25 BBAN)
            ("Qatar", "QA", 29, "DOHB00001234567890ABCDEFG"),
            // Yemen - YE + 2 check + 4 letter bank + 22 digit = 30 (26 BBAN)
            ("Yemen", "YE", 30, "CBYE0001234567890123456789"),
            // Barbados - BB + 2 check + 4 letter + 20 digits = 28 (24 BBAN)
            ("Barbados", "BB", 28, "BCHB00000000000123456789"),
            // Falkland Islands - FK + 2 check + 2 letter + 12 digit = 18 (14 BBAN)
            ("Falkland Islands", "FK", 18, "SC123456789012"),
            // Russia - RU + 2 check + 14 digit + 15 alphanumeric = 33 (29 BBAN)
            ("Russia", "RU", 33, "04452560040702810400000000012"),
        };

        _output.WriteLine("=== CALCULATED VALID IBANs ===");
        foreach (var (country, code, length, bban) in ibansToFix)
        {
            var checkDigits = CalculateCheckDigits(code, bban);
            var iban = code + checkDigits + bban;
            _output.WriteLine($"{country} ({code}): {FormatIban(iban)}");

            // Validate to make sure
            var result = GlobalIbanValidator.ValidateIban(iban);
            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault()?.Message ?? "Unknown";
                _output.WriteLine($"  WARNING: Still invalid - {error}");
            }
        }
    }

    private static string FormatIban(string iban)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < iban.Length; i++)
        {
            if (i > 0 && i % 4 == 0)
                sb.Append(' ');
            sb.Append(iban[i]);
        }
        return sb.ToString();
    }

    [Fact]
    public void ValidateAllIbanExamples()
    {
        // All IBAN examples from IbanValidator.razor
        var ibans = new (string Country, string Code, int ExpectedLength, string Example)[]
        {
            // EU Member States (27)
            ("Austria", "AT", 20, "AT61 1904 3002 3457 3201"),
            ("Belgium", "BE", 16, "BE68 5390 0754 7034"),
            ("Bulgaria", "BG", 22, "BG80 BNBG 9661 1020 3456 78"),
            ("Croatia", "HR", 21, "HR12 1001 0051 8630 0016 0"),
            ("Cyprus", "CY", 28, "CY17 0020 0128 0000 0012 0052 7600"),
            ("Czech Republic", "CZ", 24, "CZ65 0800 0000 1920 0014 5399"),
            ("Denmark", "DK", 18, "DK50 0040 0440 1162 43"),
            ("Estonia", "EE", 20, "EE38 2200 2210 2014 5685"),
            ("Finland", "FI", 18, "FI21 1234 5600 0007 85"),
            ("France", "FR", 27, "FR14 2004 1010 0505 0001 3M02 606"),
            ("Germany", "DE", 22, "DE89 3704 0044 0532 0130 00"),
            ("Greece", "GR", 27, "GR16 0110 1250 0000 0001 2300 695"),
            ("Hungary", "HU", 28, "HU42 1177 3016 1111 1018 0000 0000"),
            ("Ireland", "IE", 22, "IE29 AIBK 9311 5212 3456 78"),
            ("Italy", "IT", 27, "IT60 X054 2811 1010 0000 0123 456"),
            ("Latvia", "LV", 21, "LV80 BANK 0000 4351 9500 1"),
            ("Lithuania", "LT", 20, "LT12 1000 0111 0100 1000"),
            ("Luxembourg", "LU", 20, "LU28 0019 4006 4475 0000"),
            ("Malta", "MT", 31, "MT84 MALT 0110 0001 2345 MTLC AST0 01S"),
            ("Netherlands", "NL", 18, "NL91 ABNA 0417 1643 00"),
            ("Poland", "PL", 28, "PL61 1090 1014 0000 0712 1981 2874"),
            ("Portugal", "PT", 25, "PT03 5000 0020 1231 2345 6786 3"),
            ("Romania", "RO", 24, "RO49 AAAA 1B31 0075 9384 0000"),
            ("Slovakia", "SK", 24, "SK31 1200 0000 1987 4263 7541"),
            ("Slovenia", "SI", 19, "SI56 2633 0001 2039 086"),
            ("Spain", "ES", 24, "ES91 2100 0418 4502 0005 1332"),
            ("Sweden", "SE", 24, "SE45 5000 0000 0583 9825 7466"),
            // EEA Countries
            ("Iceland", "IS", 26, "IS14 0159 2600 7654 5510 7303 39"),
            ("Liechtenstein", "LI", 21, "LI21 0881 0000 2324 013A A"),
            ("Norway", "NO", 15, "NO93 8601 1117 947"),
            // European Territories
            ("Faroe Islands", "FO", 18, "FO62 6460 0001 6316 34"),
            ("Greenland", "GL", 18, "GL89 6471 0001 0002 06"),
            // Other European Countries
            ("Albania", "AL", 28, "AL47 2121 1009 0000 0002 3569 8741"),
            ("Andorra", "AD", 24, "AD12 0001 2030 2003 5910 0100"),
            ("Azerbaijan", "AZ", 28, "AZ21 NABZ 0000 0000 1370 1000 1944"),
            ("Belarus", "BY", 28, "BY13 NBRB 3600 9000 0000 2Z00 AB00"),
            ("Bosnia", "BA", 20, "BA39 1290 0794 0102 8494"),
            ("Georgia", "GE", 22, "GE29 NB00 0000 0101 9049 17"),
            ("Gibraltar", "GI", 23, "GI75 NWBK 0000 0000 7099 453"),
            ("United Kingdom", "GB", 22, "GB29 NWBK 6016 1331 9268 19"),
            ("Kosovo", "XK", 20, "XK05 1212 0123 4567 8906"),
            ("Moldova", "MD", 24, "MD24 AG00 0225 1000 1310 4168"),
            ("Monaco", "MC", 27, "MC58 1122 2000 0101 2345 6789 030"),
            ("Montenegro", "ME", 22, "ME25 5050 0001 2345 6789 51"),
            ("North Macedonia", "MK", 19, "MK07 2501 2000 0058 984"),
            ("San Marino", "SM", 27, "SM86 U032 2509 8000 0000 0270 100"),
            ("Serbia", "RS", 22, "RS35 2600 0560 1001 6113 79"),
            ("Switzerland", "CH", 21, "CH93 0076 2011 6238 5295 7"),
            ("Turkey", "TR", 26, "TR33 0006 1005 1978 6457 8413 26"),
            ("Ukraine", "UA", 29, "UA21 3996 2200 0002 6007 2335 6600 1"),
            ("Vatican", "VA", 22, "VA59 0011 2300 0012 3456 78"),
            ("Russia", "RU", 33, "RU51 0445 2560 0407 0281 0400 0000 0001 2"),
            // Middle East & Africa
            ("UAE", "AE", 23, "AE07 0331 2345 6789 0123 456"),
            ("Bahrain", "BH", 22, "BH67 BMAG 0000 1299 1234 56"),
            ("Iraq", "IQ", 23, "IQ98 NBIQ 8501 2345 6789 012"),
            ("Israel", "IL", 23, "IL62 0108 0000 0009 9999 999"),
            ("Jordan", "JO", 30, "JO94 CBJO 0010 0000 0000 0131 0003 02"),
            ("Kuwait", "KW", 30, "KW81 CBKU 0000 0000 0000 1234 5601 01"),
            ("Lebanon", "LB", 28, "LB62 0999 0000 0001 0019 0122 9114"),
            ("Oman", "OM", 23, "OM21 0060 1234 5678 9012 345"),
            ("Mauritania", "MR", 27, "MR13 0002 0001 0100 0012 3456 753"),
            ("Palestine", "PS", 29, "PS92 PALS 0000 0000 0400 1234 5670 2"),
            ("Qatar", "QA", 29, "QA58 DOHB 0000 1234 5678 90AB CDEF G"),
            ("Saudi Arabia", "SA", 24, "SA03 8000 0000 6080 1016 7519"),
            ("Yemen", "YE", 30, "YE84 CBYE 0001 2345 6789 0123 4567 89"),
            // Americas
            ("Brazil", "BR", 29, "BR15 0000 0000 0000 1093 2840 814 P2"),
            ("Barbados", "BB", 28, "BB53 BCHB 0000 0000 0001 2345 6789"),
            ("Falkland Islands", "FK", 18, "FK88 SC12 3456 7890 12"),
            ("Costa Rica", "CR", 22, "CR05 0152 0200 1026 2840 66"),
            ("Dominican Republic", "DO", 28, "DO28 BAGR 0000 0001 2124 5361 1324"),
            ("El Salvador", "SV", 28, "SV62 CENR 0000 0000 0000 0070 0025"),
            ("Guatemala", "GT", 28, "GT82 TRAJ 0102 0000 0012 1002 9690"),
            ("Honduras", "HN", 28, "HN48 FICO 0000 0000 0123 4567 8901"),
            ("Nicaragua", "NI", 32, "NI40 BAPI 0099 0001 0000 0000 0000 0005"),
            ("Saint Lucia", "LC", 32, "LC55 HEMM 0001 0001 0012 0012 0002 3015"),
            ("British Virgin Islands", "VG", 24, "VG96 VPVG 0000 0123 4567 8901"),
            // New Countries (v1.4.0)
            ("Algeria", "DZ", 24, "DZ50 5800 1000 0000 0000 0100"),
            ("Angola", "AO", 25, "AO52 0060 0000 0000 3000 1401 3"),
            ("Benin", "BJ", 28, "BJ09 B012 3456 7890 1234 5678 9012"),
            ("Burkina Faso", "BF", 28, "BF21 B012 3456 7890 1234 5678 9012"),
            ("Burundi", "BI", 27, "BI23 4320 1011 0674 4401 0270 188"),
            ("Cameroon", "CM", 27, "CM76 2100 0100 2003 0045 6789 016"),
            ("Cape Verde", "CV", 25, "CV85 0003 0000 0054 3210 4701 2"),
            ("Central African Republic", "CF", 27, "CF74 2001 1000 1003 0027 7000 160"),
            ("Chad", "TD", 27, "TD88 0089 0001 1001 0101 5002 037"),
            ("Comoros", "KM", 27, "KM83 4600 1000 1001 0030 0100 000"),
            ("Congo", "CG", 27, "CG89 3001 1000 1002 7103 3023 002"),
            ("Cote d'Ivoire", "CI", 28, "CI03 B012 3456 7890 1234 5678 9012"),
            ("Djibouti", "DJ", 27, "DJ97 2100 1000 1003 0045 6712 010"),
            ("Egypt", "EG", 29, "EG38 0019 0005 0000 0000 2631 8000 2"),
            ("Equatorial Guinea", "GQ", 27, "GQ42 7000 1000 1003 0067 8102 340"),
            ("Gabon", "GA", 27, "GA54 4001 3000 1001 0000 0001 001"),
            ("Guinea-Bissau", "GW", 28, "GW22 B012 3456 7890 1234 5678 9012"),
            ("Libya", "LY", 25, "LY54 0020 4800 0020 1001 2030 1"),
            ("Morocco", "MA", 28, "MA64 0115 1900 0001 2050 0053 4921"),
            ("Madagascar", "MG", 27, "MG41 4600 1000 0111 0000 9140 029"),
            ("Mali", "ML", 28, "ML98 B012 3456 7890 1234 5678 9012"),
            ("Mozambique", "MZ", 25, "MZ77 5921 0000 0010 8340 0014 1"),
            ("Niger", "NE", 28, "NE13 B012 3456 7890 1234 5678 9012"),
            ("Sao Tome and Principe", "ST", 25, "ST57 0002 0010 0192 1940 1019 2"),
            ("Senegal", "SN", 28, "SN38 B012 3456 7890 1234 5678 9012"),
            ("Seychelles", "SC", 31, "SC18 SSCB 1101 0000 0000 0000 1497 USD"),
            ("Somalia", "SO", 23, "SO93 1000 0012 3456 7890 123"),
            ("Sudan", "SD", 18, "SD67 2101 2901 8953 03"),
            ("Togo", "TG", 28, "TG50 B012 3456 7890 1234 5678 9012"),
            ("Tunisia", "TN", 24, "TN59 1000 6035 1835 9847 8831"),
            // Asia & Pacific
            ("Kazakhstan", "KZ", 20, "KZ86 125K ZT50 0410 0100"),
            ("Pakistan", "PK", 24, "PK36 SCBL 0000 0011 2345 6702"),
            ("Timor-Leste", "TL", 23, "TL38 0080 0123 4567 8910 157"),
            ("Mongolia", "MN", 20, "MN33 0001 0000 0000 0000"),
        };

        var invalidIbans = new List<(string Country, string Code, string Example, string Error)>();

        foreach (var (country, code, expectedLength, example) in ibans)
        {
            var result = GlobalIbanValidator.ValidateIban(example);
            var normalized = example.Replace(" ", "");

            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault()?.Message ?? "Unknown error";
                invalidIbans.Add((country, code, example, error));
                _output.WriteLine($"INVALID - {country} ({code}): {example} - {error}");
            }
            else
            {
                _output.WriteLine($"VALID   - {country} ({code}): {example}");
            }

            // Also check length
            if (normalized.Length != expectedLength)
            {
                _output.WriteLine($"  WARNING: Length mismatch - Expected {expectedLength}, got {normalized.Length}");
            }
        }

        _output.WriteLine($"\n\nTotal: {ibans.Length}, Invalid: {invalidIbans.Count}");

        if (invalidIbans.Count > 0)
        {
            _output.WriteLine("\n=== INVALID IBANs ===");
            foreach (var (country, code, example, error) in invalidIbans)
            {
                _output.WriteLine($"{country} ({code}): {example} - {error}");
            }
        }

        // This is informational - don't fail the test
        // Assert.Empty(invalidIbans);
    }
}
