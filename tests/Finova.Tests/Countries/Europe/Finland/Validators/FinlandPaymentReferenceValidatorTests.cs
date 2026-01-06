using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Finland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Finland.Validators;

public class FinlandPaymentReferenceValidatorTests
{
    private readonly FinlandPaymentReferenceValidator _validator;

    public FinlandPaymentReferenceValidatorTests()
    {
        _validator = new FinlandPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("1234561")] // Valid Finnish reference
    [InlineData("1009")] // Valid Finnish reference
    [InlineData("1232")] // Valid Finnish reference
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
    [InlineData("1234560")] // Wrong check digit
    [InlineData("123")] // Too short
    [InlineData("123456789012345678901")] // Too long
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalFinlandFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "1234561";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1234561", "123456", PaymentReferenceFormat.LocalFinland)]
    [InlineData("1009", "100", PaymentReferenceFormat.LocalFinland)]
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
    [InlineData("1234560")] // Invalid check digit
    public void Parse_WithInvalidReference_ReturnsNull(string? reference)
    {
        // Act
        var result = _validator.Parse(reference);

        // Assert
        result.Should().BeNull();
    }
}
