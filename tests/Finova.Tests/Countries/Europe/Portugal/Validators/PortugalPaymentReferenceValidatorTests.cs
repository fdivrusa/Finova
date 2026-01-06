using Finova.Countries.Europe.Portugal.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Portugal.Validators;

public class PortugalPaymentReferenceValidatorTests
{
    private readonly PortugalPaymentReferenceValidator _validator;

    public PortugalPaymentReferenceValidatorTests()
    {
        _validator = new PortugalPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("123456789")] // 9 digits
    [InlineData("000000000")]
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
    [InlineData("12345678")] // Too short
    [InlineData("1234567890")] // Too long
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalPortugalFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "123456789";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
