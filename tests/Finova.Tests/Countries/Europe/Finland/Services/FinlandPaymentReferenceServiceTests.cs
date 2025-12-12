using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Finland.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Finland.Services;

public class FinlandPaymentReferenceServiceTests
{
    private readonly FinlandPaymentReferenceService _service;

    public FinlandPaymentReferenceServiceTests()
    {
        _service = new FinlandPaymentReferenceService();
    }

    [Fact]
    public void CountryCode_ShouldBeFI()
    {
        _service.CountryCode.Should().Be("FI");
    }

    #region Generate Tests

    [Theory]
    [InlineData("123456", "1234561")] // Example from calculation: 123456 -> 1
    [InlineData("100", "1009")] // 1*7 + 0*3 + 0*1 = 7. 10-7=3. Wait. 100: 1*1 + 0*3 + 0*7 = 1. 10-1=9. Correct.
    [InlineData("123", "1232")] // 1*1 + 2*3 + 3*7 = 1+6+21=28. 30-28=2. Correct.
    public void Generate_WithLocalFinlandFormat_ReturnsCorrectReference(string input, string expected)
    {
        // Act
        var result = _service.Generate(input, PaymentReferenceFormat.LocalFinland);

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

    [Fact]
    public void Generate_WithUnsupportedFormat_ThrowsException()
    {
        // Act
        Action act = () => _service.Generate("123", PaymentReferenceFormat.LocalBelgian);

        // Assert
        act.Should().Throw<NotSupportedException>()
           .WithMessage("*not supported by FI*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("12")] // Too short (min 3 digits data -> 4 total)
    [InlineData("12345678901234567890")] // Too long (max 19 digits data -> 20 total)
    public void Generate_WithInvalidInput_ThrowsArgumentException(string? input)
    {
        // Act
        Action act = () => _service.Generate(input!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #region IsValid Tests

    [Theory]
    [InlineData("1234561")]
    [InlineData("1009")]
    [InlineData("1232")]
    public void IsValid_WithValidFinnishReference_ReturnsTrue(string reference)
    {
        // Act
        var result = FinlandPaymentReferenceService.ValidateStatic(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1234560")] // Wrong check digit (should be 1)
    [InlineData("123")] // Too short
    [InlineData("123456789012345678901")] // Too long
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_WithInvalidFinnishReference_ReturnsFalse(string? reference)
    {
        // Act
        var result = FinlandPaymentReferenceService.ValidateStatic(reference!);

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

