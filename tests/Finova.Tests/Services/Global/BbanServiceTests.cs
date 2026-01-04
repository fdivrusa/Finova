using Finova.Core.Identifiers;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Services.Global;

/// <summary>
/// Unit tests for <see cref="BbanService"/>.
/// </summary>
public class BbanServiceTests
{
    private readonly IBbanService _service;

    public BbanServiceTests()
    {
        // Use DI to get all registered validators
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        Finova.Extensions.ServiceCollectionExtensions.AddFinova(services);
        var provider = services.BuildServiceProvider();
        _service = provider.GetRequiredService<IBbanService>();
    }

    [Theory]
    [InlineData("BE", "539007547034", true)]
    [InlineData("BE", "123456789012", false)]
    [InlineData("BE", "000000000097", true)]
    [InlineData("FR", "30006000011234567890189", true)]
    [InlineData("FR", "30003024520000372620199", false)]
    [InlineData("DE", "370400440532013000", true)]
    [InlineData("DE", "123", false)]
    [InlineData("NL", "ABNA0417164300", true)]
    [InlineData("NL", "1234567890", false)]
    [InlineData("GB", "NWBK60161331926819", true)]
    [InlineData("GB", "123456", false)]
    [InlineData("ES", "21000418450200051332", true)]
    [InlineData("ES", "12345678901234567890", false)]
    [InlineData("IT", "X0542811101000000123456", true)]
    [InlineData("IT", "1234567890", false)]
    [InlineData("CH", "04835012345678009", true)]
    [InlineData("CH", "123", false)]
    [InlineData("NO", "86011117947", true)]
    [InlineData("NO", "12345", false)]
    [InlineData("SE", "50000000058398257466", true)]
    [InlineData("SE", "123", false)]
    [InlineData("DK", "00400440116243", true)]
    [InlineData("DK", "123", false)]
    [InlineData("FI", "12345600000785", true)]
    [InlineData("FI", "123", false)]
    [InlineData("PL", "109010140000071219812874", true)]
    [InlineData("PL", "123", false)]
    [InlineData("PT", "500000201231234567863", true)]
    [InlineData("PT", "123", false)]
    public void Validate_ShouldReturnExpectedResult(string countryCode, string bban, bool expectedIsValid)
    {
        // Act
        var result = _service.Validate(countryCode, bban);

        // Assert
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Validate_WithNullCountryCode_ShouldReturnFailure()
    {
        // Act
        var result = _service.Validate(null!, "123456789");

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WithEmptyCountryCode_ShouldReturnFailure()
    {
        // Act
        var result = _service.Validate("", "123456789");

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WithUnsupportedCountry_ShouldReturnUnsupportedCountryError()
    {
        // Act
        var result = _service.Validate("XX", "123456789");

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == Finova.Core.Common.ValidationErrorCode.UnsupportedCountry);
    }

    [Theory]
    [InlineData("BE", "539007547034")]
    [InlineData("FR", "30006000011234567890189")]
    [InlineData("DE", "370400440532013000")]
    [InlineData("GB", "NWBK60161331926819")]
    public void Parse_WithValidBban_ShouldReturnDetails(string countryCode, string bban)
    {
        // Act
        var result = _service.Parse(countryCode, bban);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(countryCode.ToUpperInvariant(), result.CountryCode);
        Assert.NotNull(result.Bban);
    }

    [Fact]
    public void Parse_WithInvalidBban_ShouldReturnNull()
    {
        // Act
        var result = _service.Parse("BE", "invalid");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Parse_WithNullBban_ShouldReturnNull()
    {
        // Act
        var result = _service.Parse("BE", null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Parse_WithUnsupportedCountry_ShouldReturnNull()
    {
        // Act
        var result = _service.Parse("XX", "123456789");

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("be", "539007547034")]
    [InlineData("Be", "539007547034")]
    [InlineData("BE", "539007547034")]
    public void Validate_ShouldBeCaseInsensitiveForCountryCode(string countryCode, string bban)
    {
        // Act
        var result = _service.Validate(countryCode, bban);

        // Assert
        Assert.True(result.IsValid);
    }
}
