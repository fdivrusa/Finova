using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Switzerland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Switzerland.Validators;

public class SwitzerlandPaymentReferenceValidatorTests
{
    private readonly SwitzerlandPaymentReferenceValidator _validator;

    public SwitzerlandPaymentReferenceValidatorTests()
    {
        _validator = new SwitzerlandPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("210000000003139471430009017")] // Valid QR-Reference
    [InlineData("000000000000000000000001236")] // Valid QR-Reference
    public void Validate_WithValidReference_ReturnsSuccess(string reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("210000000003139471430009010")] // Invalid check digit
    [InlineData("123")] // Too short
    [InlineData("2100000000031394714300090171")] // Too long
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalSwitzerlandFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "210000000003139471430009017";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("210000000003139471430009017", "21000000000313947143000901", PaymentReferenceFormat.LocalSwitzerland)]
    [InlineData("000000000000000000000001236", "00000000000000000000000123", PaymentReferenceFormat.LocalSwitzerland)]
    public void Parse_WithValidReference_ReturnsDetails(string reference, string expectedContent, PaymentReferenceFormat expectedFormat)
    {
        // Act
        var result = _validator.Parse(reference);

        // Assert
        result.Should().NotBeNull();
        result!.Reference.Should().Be(reference);
        result.Content.Should().Be(expectedContent);
        result.Format.Should().Be(expectedFormat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("210000000003139471430009010")] // Invalid check digit
    public void Parse_WithInvalidReference_ReturnsNull(string? reference)
    {
        // Act
        var result = _validator.Parse(reference);

        // Assert
        result.Should().BeNull();
    }
}
