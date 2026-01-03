using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services.Global;

public class GlobalIbanValidatorTests
{
    #region European IBANs
    [Theory]
    [InlineData("DE89370400440532013000")] // Germany
    [InlineData("FR7630006000011234567890189")] // France
    [InlineData("GB82WEST12345698765432")] // United Kingdom
    [InlineData("ES9121000418450200051332")] // Spain
    [InlineData("IT60X0542811101000000123456")] // Italy
    [InlineData("NL91ABNA0417164300")] // Netherlands
    [InlineData("BE68539007547034")] // Belgium
    [InlineData("AT611904300234573201")] // Austria
    [InlineData("CH9300762011623852957")] // Switzerland
    [InlineData("PL61109010140000071219812874")] // Poland
    public void ValidateIban_WithValidEuropeanIbans_ReturnsTrue(string iban)
        => GlobalIbanValidator.ValidateIban(iban).IsValid.Should().BeTrue();
    #endregion

    #region Middle East IBANs
    [Theory]
    [InlineData("BH67BMAG00001299123456")] // Bahrain
    [InlineData("IL620108000000099999999")] // Israel
    [InlineData("JO94CBJO0010000000000131000302")] // Jordan
    [InlineData("KW81CBKU0000000000001234560101")] // Kuwait
    [InlineData("LB62099900000001001901229114")] // Lebanon
    [InlineData("QA58DOHB00001234567890ABCDEFG")] // Qatar
    [InlineData("SA0380000000608010167519")] // Saudi Arabia
    [InlineData("AE070331234567890123456")] // UAE
    public void ValidateIban_WithValidMiddleEastIbans_ReturnsTrue(string iban)
        => GlobalIbanValidator.ValidateIban(iban).IsValid.Should().BeTrue();
    #endregion

    #region Africa IBANs
    [Theory]
    [InlineData("EG380019000500000000263180002")] // Egypt
    [InlineData("MR1300020001010000123456753")] // Mauritania
    public void ValidateIban_WithValidAfricaIbans_ReturnsTrue(string iban)
        => GlobalIbanValidator.ValidateIban(iban).IsValid.Should().BeTrue();
    #endregion

    #region Americas IBANs
    [Theory]
    [InlineData("BR1800360305000010009795493C1")] // Brazil
    [InlineData("CR05015202001026284066")] // Costa Rica
    [InlineData("DO22ACAU00000000000123456789")] // Dominican Republic
    [InlineData("SV62CENR00000000000000700025")] // El Salvador
    [InlineData("GT82TRAJ01020000001210029690")] // Guatemala
    [InlineData("VG96VPVG0000012345678901")] // Virgin Islands British
    public void ValidateIban_WithValidAmericasIbans_ReturnsTrue(string iban)
        => GlobalIbanValidator.ValidateIban(iban).IsValid.Should().BeTrue();
    #endregion

    #region Asia IBANs
    [Theory]
    [InlineData("KZ86125KZT5004100100")] // Kazakhstan
    [InlineData("PK36SCBL0000001123456702")] // Pakistan
    [InlineData("TL380080012345678910157")] // Timor-Leste
    public void ValidateIban_WithValidAsiaIbans_ReturnsTrue(string iban)
        => GlobalIbanValidator.ValidateIban(iban).IsValid.Should().BeTrue();
    #endregion

    #region Invalid IBANs
    [Theory]
    [InlineData("XX89370400440532013000")] // Invalid country code
    [InlineData("DE00370400440532013000")] // Wrong check digits
    [InlineData("DE8937040044053201")] // Too short
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ValidateIban_WithInvalidIbans_ReturnsFalse(string? iban)
        => GlobalIbanValidator.ValidateIban(iban).IsValid.Should().BeFalse();
    #endregion

    #region Formatted IBANs
    [Theory]
    [InlineData("DE89 3704 0044 0532 0130 00")] // With spaces
    [InlineData("de89370400440532013000")] // Lowercase
    [InlineData("  DE89370400440532013000  ")] // With leading/trailing spaces
    public void ValidateIban_WithFormattedIbans_ReturnsTrue(string iban)
        => GlobalIbanValidator.ValidateIban(iban).IsValid.Should().BeTrue();
    #endregion
}
