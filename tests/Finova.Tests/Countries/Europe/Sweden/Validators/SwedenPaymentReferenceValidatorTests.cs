using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Sweden.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Sweden.Validators;

public class SwedenPaymentReferenceValidatorTests
{
    private readonly SwedenPaymentReferenceValidator _validator;

    public SwedenPaymentReferenceValidatorTests()
    {
        _validator = new SwedenPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("12345682")] // Valid OCR reference
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
    [InlineData("12345680")] // Invalid check digit
    [InlineData("12345692")] // Invalid length digit
    [InlineData("1")] // Too short
    [InlineData("12345678901234567890123456")] // Too long
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalSwedenFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "12345682";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("12345682", "123456", PaymentReferenceFormat.LocalSweden)]
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
    [InlineData("12345680")] // Invalid check digit
    public void Parse_WithInvalidReference_ReturnsNull(string? reference)
    {
        // Act
        var result = _validator.Parse(reference);

        // Assert
        result.Should().BeNull();
    }
}
