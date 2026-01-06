using Finova.Countries.Europe.Italy.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Italy.Validators;

public class ItalyPaymentReferenceValidatorTests
{
    private readonly ItalyPaymentReferenceValidator _validator;

    public ItalyPaymentReferenceValidatorTests()
    {
        _validator = new ItalyPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("123456789012347")] // Calculated valid Luhn (15 digits)
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
    [InlineData("12345678901234")] // Too short (14)
    [InlineData("1234567890123456789")] // Too long (19)
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalItalyFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "123456789012345671";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
