using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class EuropeIbanValidatorTests
{
    private readonly EuropeIbanValidator _validator;

    public EuropeIbanValidatorTests()
    {
        _validator = new EuropeIbanValidator();
    }

    [Theory]
    [InlineData("AD1400080001001234567890")] // Andorra
    [InlineData("AT611904300234573201")] // Austria
    [InlineData("BE68539007547034")] // Belgium
    [InlineData("BG19STSA93000123456789")] // Bulgaria
    [InlineData("CH5604835012345678009")] // Switzerland
    [InlineData("IE29AIBK93115212345678")] // Ireland
    [InlineData("IS750001121234563108962099")] // Iceland
    [InlineData("IT60X0542811101000000123456")] // Italy
    [InlineData("LT601010012345678901")] // Lithuania
    [InlineData("LU280019400644750000")] // Luxembourg
    [InlineData("LV97HABA0012345678910")] // Latvia
    [InlineData("MC5810096180790123456789085")] // Monaco
    [InlineData("MT31MALT01100000000000000000123")] // Malta
    [InlineData("NL91ABNA0417164300")] // Netherlands
    [InlineData("NO9386011117947")] // Norway
    [InlineData("PL61109010140000071219812874")] // Poland
    [InlineData("PT03500000201231234567863")] // Portugal
    [InlineData("RO49AAAA1B31007593840000")] // Romania
    [InlineData("SE4550000000058398257466")] // Sweden
    [InlineData("SI56192001234567892")] // Slovenia
    [InlineData("SK8975000000000012345671")] // Slovakia
    [InlineData("SM76P0854009812123456789123")] // San Marino
    [InlineData("VA59001123000012345678")] // Vatican
    [InlineData("AL96100100010000000000000001")] // Albania
    [InlineData("AZ21NABZ00000000137010001944")] // Azerbaijan
    [InlineData("BY29BAPB30000000000000000001")] // Belarus
    [InlineData("BA783930000000000000")] // Bosnia and Herzegovina
    [InlineData("FO7460000000000011")] // Faroe Islands
    [InlineData("GE6329NB00000001019049")] // Georgia
    [InlineData("GL5360000000000001")] // Greenland
    [InlineData("XK950505000000000000")] // Kosovo
    [InlineData("LI2200000000000000001")] // Liechtenstein
    [InlineData("MD76AA100000000000000001")] // Moldova
    [InlineData("ME36500000000000000001")] // Montenegro
    [InlineData("MK31100000000000001")] // North Macedonia
    [InlineData("TR960006100000000000000001")] // Turkey
    [InlineData("UA443000230000000000000000000")] // Ukraine
    [InlineData("RS35260005601001611379")] // Serbia
    public void Validate_WithValidIban_ReturnsTrue(string iban)
    {
        // Act
        // Act
        var result = _validator.Validate(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithNullIban_ReturnsFalse()
    {
        _validator.Validate(null).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidIban_ReturnsFalse()
    {
        _validator.Validate("invalid").IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithUnsupportedCountry_ReturnsFalse()
    {
        _validator.Validate("ZZ000000000000000000").IsValid.Should().BeFalse();
    }
}

