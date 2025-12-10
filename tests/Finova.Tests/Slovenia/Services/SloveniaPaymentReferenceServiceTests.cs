using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Slovenia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Slovenia.Services;

public class SloveniaPaymentReferenceServiceTests
{
    private readonly SloveniaPaymentReferenceService _service;

    public SloveniaPaymentReferenceServiceTests()
    {
        _service = new SloveniaPaymentReferenceService();
    }

    [Fact]
    public void CountryCode_ShouldBeSI()
    {
        _service.CountryCode.Should().Be("SI");
    }

    #region Generate Tests

    [Theory]
    [InlineData("100", "SI1210003")]
    public void Generate_WithLocalSloveniaFormat_ReturnsCorrectReference(string input, string expected)
    {
        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.LocalSlovenia);
        result.Should().Be(expected);
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
    [InlineData("SI1212345672")] // Valid from calculation
    [InlineData("SI1210003")] // Valid from calculation
    public void IsValid_WithValidSloveniaReference_ReturnsTrue(string reference)
    {
        // Act
        var result = SloveniaPaymentReferenceService.ValidateStatic(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SI1212345600")] // Invalid check digit (should be 72)
    [InlineData("SI12")] // Too short
    [InlineData("123456")] // Missing prefix
    [InlineData("XX1212345672")] // Wrong prefix
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_WithInvalidSloveniaReference_ReturnsFalse(string? reference)
    {
        // Act
        var result = SloveniaPaymentReferenceService.ValidateStatic(reference!);

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
