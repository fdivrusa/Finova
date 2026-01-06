using Finova.Services;
using Xunit;
using Xunit.Abstractions;

namespace Finova.Tests;

public class BusinessExampleTests
{
    private readonly ITestOutputHelper _output;

    public BusinessExampleTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData("AT", "FN 123456x")]
    [InlineData("BE", "0202.239.951")]
    [InlineData("BG", "131468980")]
    [InlineData("CH", "CHE-105.815.381")]
    [InlineData("CY", "10259033P")]
    [InlineData("CZ", "00177041")]
    [InlineData("DE", "HRB 12345")]
    [InlineData("DK", "47458714")]
    [InlineData("EE", "10000356")]
    [InlineData("ES", "A58818501")]
    [InlineData("FI", "2058430-6")]
    [InlineData("FR", "732 829 320")]
    [InlineData("GB", "02204302")]
    [InlineData("EL", "094019245")]
    [InlineData("HR", "81793146560")]
    [InlineData("HU", "10000001111")]
    [InlineData("IE", "6388047V")]
    [InlineData("IT", "00159560366")]
    [InlineData("LT", "110053842")]
    [InlineData("LU", "10000356")]
    [InlineData("LV", "40003245752")]
    [InlineData("MT", "12345671")]
    [InlineData("NL", "123456782B01")]
    [InlineData("NO", "923609016")]
    [InlineData("PL", "5260250995")]
    [InlineData("PT", "500278725")]
    [InlineData("RO", "123458")]
    [InlineData("SE", "556728583701")]
    [InlineData("SI", "15012557")]
    [InlineData("SK", "1100000000")]
    [InlineData("TR", "1234567890")]
    public void VerifyExample(string countryCode, string example)
    {
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(example, countryCode);
        if (!result.IsValid)
        {
            _output.WriteLine($"Failed: {countryCode} - {example}: {result.Errors.FirstOrDefault()?.Message}");
        }
        Assert.True(result.IsValid, $"Example for {countryCode} ({example}) should be valid but failed: {result.Errors.FirstOrDefault()?.Message}");
    }
}
