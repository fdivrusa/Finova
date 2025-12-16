using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Denmark.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Denmark.Validators;

public class DenmarkPaymentReferenceValidatorTests
{
    private readonly DenmarkPaymentReferenceValidator _validator;

    public DenmarkPaymentReferenceValidatorTests()
    {
        _validator = new DenmarkPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("123456789012347")] // Calculated valid Luhn
    [InlineData("+71<123456789012347")] // With formatting
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
    [InlineData("123456789012342")] // Invalid check digit
    [InlineData("123")] // Too short
    [InlineData("1234567890123456789")] // Too long
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalDenmarkFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "+71<123456789012347";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
