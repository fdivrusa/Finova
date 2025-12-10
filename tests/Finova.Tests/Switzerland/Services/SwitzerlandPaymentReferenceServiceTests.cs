using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Switzerland.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Switzerland.Services;

public class SwitzerlandPaymentReferenceServiceTests
{
    private readonly SwitzerlandPaymentReferenceService _service;

    public SwitzerlandPaymentReferenceServiceTests()
    {
        _service = new SwitzerlandPaymentReferenceService();
    }

    [Fact]
    public void CountryCode_ShouldBeCH()
    {
        _service.CountryCode.Should().Be("CH");
    }

    #region Generate Tests

    [Theory]
    [InlineData("21000000000313947143000901", "210000000003139471430009017")]
    // Example from Swiss QR Bill documentation
    // Ref: 21 00000 00003 13947 14300 09017
    // Data: 21000000000313947143000901
    // Check Digit: 7
    public void Generate_WithLocalSwitzerlandFormat_ReturnsCorrectReference(string input, string expected)
    {
        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.LocalSwitzerland);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Generate_WithShortInput_PadsTo27Digits()
    {
        // Arrange
        var input = "123";
        var expected = "000000000000000000000001236";

        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.LocalSwitzerland);

        // Assert
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
    [InlineData("210000000003139471430009017")] // Valid
    [InlineData("000000000000000000000001236")] // Valid
    public void IsValid_WithValidSwitzerlandReference_ReturnsTrue(string reference)
    {
        // Act
        var result = SwitzerlandPaymentReferenceService.ValidateStatic(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("210000000003139471430009010")] // Invalid check digit
    [InlineData("123")] // Too short (must be 27)
    [InlineData("2100000000031394714300090171")] // Too long (28)
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_WithInvalidSwitzerlandReference_ReturnsFalse(string? reference)
    {
        // Act
        var result = SwitzerlandPaymentReferenceService.ValidateStatic(reference!);

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
