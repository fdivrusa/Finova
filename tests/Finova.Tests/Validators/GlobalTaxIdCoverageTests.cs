using Finova.Core.Identifiers;
using Finova.Extensions.DependencyInjection;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Validators;

public class GlobalTaxIdCoverageTests
{
    private readonly ITaxIdService _service;

    public GlobalTaxIdCoverageTests()
    {
        var services = new ServiceCollection();
        services.AddFinovaAfrica();
        services.AddFinovaAsia();
        services.AddFinovaEurope();
        services.AddFinovaNorthAmerica();
        services.AddFinovaSouthAmerica();
        services.AddFinovaSoutheastAsia();
        services.AddSingleton<ITaxIdService, TaxIdService>();

        var provider = services.BuildServiceProvider();
        _service = provider.GetRequiredService<ITaxIdService>();
    }

    [Theory]
    [InlineData("RU", "7707083893")] // Russia
    [InlineData("MA", "001525487000088")] // Morocco
    [InlineData("DZ", "000016001275946")] // Algeria
    [InlineData("TN", "1234567A/B/M/000")] // Tunisia
    [InlineData("EG", "100200300")] // Egypt
    [InlineData("KZ", "980540003232")] // Kazakhstan
    [InlineData("VN", "0100109106")] // Vietnam
    [InlineData("NG", "12345678")] // Nigeria
    [InlineData("AO", "1234567890")] // Angola
    [InlineData("SN", "123456789012345")] // Senegal
    [InlineData("CI", "1234567A")] // Ivory Coast
    [InlineData("CR", "1234567890")] // Costa Rica
    [InlineData("DO", "123456789")] // Dominican Republic
    [InlineData("HN", "12345678901234")] // Honduras
    [InlineData("NI", "12345678901234")] // Nicaragua
    [InlineData("SV", "12345678901234")] // El Salvador
    [InlineData("GT", "12345678")] // Guatemala
    public void Verify_TaxId_Service_Supports_Country(string code, string taxId)
    {
        var result = _service.Validate(code, taxId);
        Assert.True(result.IsValid, $"Tax ID for {code} should be valid. Error: {result.Errors.FirstOrDefault()?.Message}");
    }
}
