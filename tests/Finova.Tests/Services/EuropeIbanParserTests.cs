using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class EuropeIbanParserTests
{
    private readonly EuropeIbanParser _parser;

    public EuropeIbanParserTests()
    {
        _parser = new EuropeIbanParser();
    }

    [Theory]
    [InlineData("AD1400080001001234567890", "AD")] // Andorra
    [InlineData("AT611904300234573201", "AT")] // Austria
    [InlineData("BE68539007547034", "BE")] // Belgium
    [InlineData("BG19STSA93000123456789", "BG")] // Bulgaria
    [InlineData("CH5604835012345678009", "CH")] // Switzerland
    [InlineData("CY21002001950000357001234567", "CY")] // Cyprus
    [InlineData("CZ6508000000192000145399", "CZ")] // Czech Republic
    [InlineData("DE89370400440532013000", "DE")] // Germany
    [InlineData("DK5000400440116243", "DK")] // Denmark
    [InlineData("EE201000001020145686", "EE")] // Estonia
    [InlineData("ES9121000418450200051332", "ES")] // Spain
    [InlineData("FI3212345600000005", "FI")] // Finland
    [InlineData("FR1420041010050500013M02606", "FR")] // France
    [InlineData("GB29NWBK60161331926819", "GB")] // United Kingdom
    [InlineData("GI56XAPO000001234567890", "GI")] // Gibraltar
    [InlineData("GR1601101250000000012300695", "GR")] // Greece
    [InlineData("HR1723600001101234565", "HR")] // Croatia
    [InlineData("HU42117730161111101800000000", "HU")] // Hungary
    [InlineData("IE29AIBK93115212345678", "IE")] // Ireland
    [InlineData("IS750001121234563108962099", "IS")] // Iceland
    [InlineData("IT60X0542811101000000123456", "IT")] // Italy
    [InlineData("LT601010012345678901", "LT")] // Lithuania
    [InlineData("LU280019400644750000", "LU")] // Luxembourg
    [InlineData("LV97HABA0012345678910", "LV")] // Latvia
    [InlineData("MC5810096180790123456789085", "MC")] // Monaco
    [InlineData("MT31MALT01100000000000000000123", "MT")] // Malta
    [InlineData("NL91ABNA0417164300", "NL")] // Netherlands
    [InlineData("NO9386011117947", "NO")] // Norway
    [InlineData("PL61109010140000071219812874", "PL")] // Poland
    [InlineData("PT03500000201231234567863", "PT")] // Portugal
    [InlineData("RO49AAAA1B31007593840000", "RO")] // Romania
    [InlineData("SE4550000000058398257466", "SE")] // Sweden
    [InlineData("SI56192001234567892", "SI")] // Slovenia
    [InlineData("SK8975000000000012345671", "SK")] // Slovakia
    [InlineData("SM76P0854009812123456789123", "SM")] // San Marino
    [InlineData("VA59001123000012345678", "VA")] // Vatican
    [InlineData("AL96100100010000000000000001", "AL")] // Albania
    [InlineData("AZ21NABZ00000000137010001944", "AZ")] // Azerbaijan
    [InlineData("BY29BAPB30000000000000000001", "BY")] // Belarus
    [InlineData("BA783930000000000000", "BA")] // Bosnia and Herzegovina
    [InlineData("FO7460000000000011", "FO")] // Faroe Islands
    [InlineData("GE6329NB00000001019049", "GE")] // Georgia
    [InlineData("GL5360000000000001", "GL")] // Greenland
    [InlineData("XK950505000000000000", "XK")] // Kosovo
    [InlineData("LI2200000000000000001", "LI")] // Liechtenstein
    [InlineData("MD76AA100000000000000001", "MD")] // Moldova
    [InlineData("ME36500000000000000001", "ME")] // Montenegro
    [InlineData("MK31100000000000001", "MK")] // North Macedonia
    [InlineData("TR960006100000000000000001", "TR")] // Turkey
    [InlineData("UA443000230000000000000000000", "UA")] // Ukraine
    [InlineData("RS35260005601001611379", "RS")] // Serbia
    public void Parse_WithValidIban_ReturnsCorrectDetails(string iban, string expectedCountryCode)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be(expectedCountryCode);
    }

    [Fact]
    public void Parse_WithNullIban_ReturnsNull()
    {
        _parser.ParseIban(null).Should().BeNull();
    }

    [Fact]
    public void Parse_WithInvalidIban_ReturnsNull()
    {
        _parser.ParseIban("invalid").Should().BeNull();
    }

    [Fact]
    public void Parse_WithUnsupportedCountry_ReturnsNull()
    {
        _parser.ParseIban("ZZ000000000000000000").Should().BeNull();
    }
}
