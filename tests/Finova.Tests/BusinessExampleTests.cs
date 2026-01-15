using Finova.Services;
using Xunit;
using Xunit.Abstractions;

namespace Finova.Tests;

public class BusinessExampleTests
{
    private readonly ITestOutputHelper _output;
    private readonly GlobalEnterpriseValidator _validator;

    public BusinessExampleTests(ITestOutputHelper output)
    {
        _output = output;
        _validator = new GlobalEnterpriseValidator();
    }

    [Theory]
    [InlineData("AL", "K12345678L")]       // Albania NIPT
    [InlineData("AD", "F-123456-Z")]       // Andorra NRT
    [InlineData("AT", "FN 123456x")]
    [InlineData("AZ", "1234567890")]       // Azerbaijan VOEN
    [InlineData("BY", "100185330")]        // Belarus UNP
    [InlineData("BE", "0202.239.951")]
    [InlineData("BA", "4200000000004")]    // Bosnia JIB
    [InlineData("BG", "131468980")]
    [InlineData("CH", "CHE-105.815.381")]
    [InlineData("CY", "10259033P")]
    [InlineData("CZ", "00177041")]
    [InlineData("DE", "HRB 12345")]
    [InlineData("DK", "47458714")]
    [InlineData("EE", "10000356")]
    [InlineData("ES", "A58818501")]
    [InlineData("FO", "123451")]           // Faroe Islands V-TAL
    [InlineData("FI", "2058430-6")]
    [InlineData("FR", "732 829 320")]
    [InlineData("GE", "204522229")]
    [InlineData("GB", "02204302")]
    [InlineData("GI", "123456")]           // Gibraltar Company Number
    [InlineData("GL", "12345674")]
    [InlineData("EL", "094019245")]
    [InlineData("HR", "81793146560")]
    [InlineData("HU", "10000001111")]
    [InlineData("IS", "4101302979")]       // Iceland Kennitala (Organization)
    [InlineData("IE", "123456")]           // Irish CRO - 6 digits
    [InlineData("IT", "00159560366")]
    [InlineData("XK", "600000000")]
    [InlineData("LT", "110053842")]
    [InlineData("LU", "B123456")]          // Luxembourg RCS - letter + digits
    [InlineData("LV", "40003245752")]
    [InlineData("LI", "FL-105.815.381")]
    [InlineData("MK", "4030992250004")]    // North Macedonia EDB
    [InlineData("MT", "C12345")]           // Malta Company Number - C + 5 digits
    [InlineData("MD", "1000000000007")]    // Moldova IDNO
    [InlineData("MC", "23S12345")]         // Monaco RCI
    [InlineData("ME", "10000004")]         // Montenegro PIB
    [InlineData("NL", "12345678")]         // Netherlands KvK - 8 digits
    [InlineData("NO", "923609016")]
    [InlineData("PL", "5260250995")]
    [InlineData("PT", "500278725")]
    [InlineData("RO", "123458")]
    [InlineData("RU", "7707083893")]
    [InlineData("SM", "12345")]            // San Marino COE
    [InlineData("RS", "100000016")]        // Serbia PIB
    [InlineData("SE", "5560360793")]       // Swedish Org number - 10 digits with valid Luhn (IKEA)
    [InlineData("SI", "1234567400")]       // Slovenia Matična - 10 digits with valid check
    [InlineData("SK", "35780622")]         // Slovakia IČO - 8 digits with valid check
    [InlineData("TR", "1234567890")]
    [InlineData("UA", "12345678")]         // Ukraine EDRPOU
    [InlineData("US", "12-3456789")]       // USA EIN
    [InlineData("CA", "123456782")]        // Canada BN
    [InlineData("BR", "00.000.000/0001-91")] // Brazil CNPJ
    [InlineData("MX", "GODE561231GR8")]    // Mexico RFC
    [InlineData("IN", "BZPPP1123P")]       // India PAN
    [InlineData("CN", "91310000633518315Q")] // China USCC
    [InlineData("DZ", "000012345678901")]  // Algeria NIF
    [InlineData("EG", "123456789")]        // Egypt TRN
    [InlineData("MA", "001234567890123")]  // Morocco ICE
    [InlineData("NG", "12345678")]         // Nigeria TIN
    [InlineData("TN", "1234567APM000")]    // Tunisia Matricule
    [InlineData("KZ", "600900563762")]     // Kazakhstan BIN
    [InlineData("VN", "0100109106")]       // Vietnam MST
    public void VerifyExample(string countryCode, string example)
    {
        var result = _validator.Validate(example, countryCode);
        if (!result.IsValid)
        {
            _output.WriteLine($"Failed: {countryCode} - {example}: {result.Errors.FirstOrDefault()?.Message}");
        }
        Assert.True(result.IsValid, $"Example for {countryCode} ({example}) should be valid but failed: {result.Errors.FirstOrDefault()?.Message}");
    }
}