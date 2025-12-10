using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Norway.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Norway.Services;

public class NorwayPaymentReferenceServiceTests
{
    private readonly NorwayPaymentReferenceService _service;

    public NorwayPaymentReferenceServiceTests()
    {
        _service = new NorwayPaymentReferenceService();
    }

    [Fact]
    public void CountryCode_ShouldBeNO()
    {
        _service.CountryCode.Should().Be("NO");
    }

    #region Generate Tests

    [Theory]
    [InlineData("12345678", "123456782")] // Mod10: 12345678 -> 2
    [InlineData("234567", "2345676")] // Mod10: 234567 -> 6
    public void Generate_WithLocalNorwayFormat_UsesMod10ByDefault(string input, string expected)
    {
        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.LocalNorway);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("12345678-11", "123456785")] // Mod11: 12345678 -> 5
    [InlineData("234567-11", "2345676")] // Mod11: 234567 -> 6
    public void Generate_WithMod11Suffix_UsesMod11(string input, string expected)
    {
        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.LocalNorway);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Generate_WithMod11Result10_ThrowsException()
    {
        // Arrange
        var input = "23-11";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _service.Generate(input, PaymentReferenceFormat.LocalNorway));
        Assert.Contains("result is 10", ex.Message);
    }

    [Fact]
    public void Generate_WithIsoRfFormat_ReturnsIsoReference()
    {
        // Arrange
        var input = "123456";

        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.IsoRf);

        // Assert
        result.Should().StartWith("RF");
        IsoPaymentReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    #endregion

    #region IsValid Tests

    [Theory]
    [InlineData("123456782")] // Mod10 valid
    [InlineData("2345676")] // Mod10/Mod11 valid
    [InlineData("123456785")] // Mod11 valid
    public void IsValid_WithValidNorwayReference_ReturnsTrue(string reference)
    {
        // Act
        var result = NorwayPaymentReferenceService.ValidateStatic(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123456780")] // Invalid check digit
    [InlineData("12")] // Too short
    [InlineData("12345678901234567890123456")] // Too long (26 digits)
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_WithInvalidNorwayReference_ReturnsFalse(string? reference)
    {
        // Act
        var result = NorwayPaymentReferenceService.ValidateStatic(reference!);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithValidIsoRfReference_ReturnsTrue()
    {
        // Arrange
        var reference = "RF18539007547034";

        // Act
        var result = IsoPaymentReferenceValidator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}
