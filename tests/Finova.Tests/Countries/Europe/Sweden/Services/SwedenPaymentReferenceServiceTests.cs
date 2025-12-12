using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Sweden.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Sweden.Services;

public class SwedenPaymentReferenceServiceTests
{
    private readonly SwedenPaymentReferenceService _service;

    public SwedenPaymentReferenceServiceTests()
    {
        _service = new SwedenPaymentReferenceService();
    }

    [Fact]
    public void CountryCode_ShouldBeSE()
    {
        _service.CountryCode.Should().Be("SE");
    }

    #region Generate Tests

    [Theory]
    [InlineData("123456", "12345682")]
    // Data: 123456
    // Length: 6 + 2 = 8. Length Digit = 8.
    // RefWithLength: 1234568
    // Luhn on 1234568:
    // 8*2=16(7), 6*1=6, 5*2=10(1), 4*1=4, 3*2=6, 2*1=2, 1*2=2
    // Sum = 7+6+1+4+6+2+2 = 28.
    // 30 - 28 = 2. Check Digit = 2.
    // Result: 12345682
    public void Generate_WithLocalSwedenFormat_ReturnsCorrectReference(string input, string expected)
    {
        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.LocalSweden);

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
    [InlineData("12345682")] // Valid from calculation
    public void IsValid_WithValidSwedenReference_ReturnsTrue(string reference)
    {
        // Act
        var result = SwedenPaymentReferenceService.ValidateStatic(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("12345680")] // Invalid check digit
    [InlineData("12345692")] // Invalid length digit (should be 8 for length 8)
    [InlineData("1")] // Too short
    [InlineData("12345678901234567890123456")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_WithInvalidSwedenReference_ReturnsFalse(string? reference)
    {
        // Act
        var result = SwedenPaymentReferenceService.ValidateStatic(reference!);

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

