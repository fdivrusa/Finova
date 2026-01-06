using Finova.Services;
using Xunit;

namespace Finova.Tests.Validators;

public class VatExamplesTest
{
    [Theory]
    [InlineData("AT", "ATU33864707")]
    [InlineData("BE", "BE0202239951")]
    [InlineData("BG", "BG131468980")]
    [InlineData("HR", "HR81793146560")]
    [InlineData("CY", "CY30010823A")]
    [InlineData("CZ", "CZ00177041")]
    [InlineData("DK", "DK47458714")]
    [InlineData("EE", "EE100366327")]
    [InlineData("FI", "FI20584306")]
    [InlineData("FR", "FR33855200507")]
    [InlineData("DE", "DE129273398")]
    [InlineData("EL", "EL094019245")]
    [InlineData("HU", "HU17781774")]
    [InlineData("IE", "IE4749148U")]
    [InlineData("IT", "IT00159560366")]
    [InlineData("LV", "LV40003245752")]
    [InlineData("LT", "LT230335113")]
    [InlineData("LU", "LU18804375")]
    [InlineData("MT", "MT26758324")]
    [InlineData("NL", "NL001786519B01")]
    [InlineData("PL", "PL7342867148")]
    [InlineData("PT", "PT500278725")]
    [InlineData("RO", "RO160796")]
    [InlineData("SK", "SK2020317068")]
    [InlineData("SI", "SI82646716")]
    [InlineData("ES", "ESA28017895")]
    [InlineData("SE", "SE556056625801")]
    [InlineData("AL", "ALK31415037M")]
    [InlineData("AD", "ADU123456B")]
    [InlineData("AZ", "AZ1234567890")]
    [InlineData("BA", "BA4000000000005")]
    [InlineData("BY", "BY100000007")]
    [InlineData("FO", "FO123456")]
    [InlineData("GB", "GB220430231")]
    [InlineData("GE", "GE123456789")]
    [InlineData("IS", "IS12345")]
    [InlineData("XK", "XK123456782")]
    [InlineData("LI", "LI123456788")]
    [InlineData("MC", "FR44732829320")]
    [InlineData("MD", "MD1234567890123")]
    [InlineData("ME", "ME10000004")]
    [InlineData("MK", "MK4030992255006")]
    [InlineData("NO", "NO923609016MVA")]
    [InlineData("RS", "RS100000024")]
    [InlineData("CHE", "CHE105815381")]
    [InlineData("SM", "SM12345")]
    [InlineData("TR", "TR1234567890")]
    [InlineData("UA", "UA12345678")]
    public void VerifyVatExample(string countryCode, string vatNumber)
    {
        var result = EuropeVatValidator.ValidateVat(vatNumber, countryCode);
        Assert.True(result.IsValid, $"VAT {vatNumber} for {countryCode} is invalid: {string.Join(", ", result.Errors.Select(e => e.Message))}");
    }
}
