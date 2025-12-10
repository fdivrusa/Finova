using Finova.Core.PaymentReference;
using Finova.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Validators;

public class PaymentReferenceValidatorTests
{
    private readonly PaymentReferenceValidator _validator;

    public PaymentReferenceValidatorTests()
    {
        _validator = new PaymentReferenceValidator();
    }

    [Theory]
    [InlineData("RF18539007547034")]
    [InlineData("RF18 5390 0754 7034")]
    public void Validate_WithValidIsoRf_ReturnsSuccess(string reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RF00539007547034")] // Invalid checksum
    [InlineData("RF1853900754703X")] // Invalid character
    public void Validate_WithInvalidIsoRf_ReturnsFailure(string reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("+++090/9337/55493+++", PaymentReferenceFormat.LocalBelgian)] // Belgian OGM
    [InlineData("1234561", PaymentReferenceFormat.LocalFinland)] // Finnish Viitenumero
    [InlineData("2345676", PaymentReferenceFormat.LocalNorway)] // Norwegian KID
    [InlineData("12345682", PaymentReferenceFormat.LocalSweden)] // Swedish OCR
    [InlineData("210000000003139471430009017", PaymentReferenceFormat.LocalSwitzerland)] // Swiss QR
    [InlineData("SI1212345672", PaymentReferenceFormat.LocalSlovenia)] // Slovenian SI12
    public void Validate_WithValidLocalFormat_ReturnsSuccess(string reference, PaymentReferenceFormat format)
    {
        // Act
        var result = _validator.Validate(reference, format);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithUnknownFormat_ReturnsFailure()
    {
        // Arrange
        var reference = "UNKNOWN-FORMAT-123";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithNullOrEmpty_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
