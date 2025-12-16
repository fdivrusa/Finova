using Finova.Belgium.Validators;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Validators;

public class BelgiumPaymentReferenceValidatorTests
{
    private readonly BelgiumPaymentReferenceValidator _validator;

    public BelgiumPaymentReferenceValidatorTests()
    {
        _validator = new BelgiumPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("+++090/9337/55493+++")] // Valid OGM
    [InlineData("090933755493")] // Valid without formatting
    [InlineData("+++101/0123/45685+++")] // Calculated valid
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
    [InlineData("+++090/9337/55494+++")] // Invalid check digit
    [InlineData("123")] // Too short
    [InlineData("1234567890123456")] // Too long
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalBelgianFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "+++090/9337/55493+++";

        // Act
        var result = _validator.Validate(reference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithUnsupportedFormat_ReturnsFailure()
    {
        // Arrange
        var reference = "+++090/9337/55493+++";

        // Act
        var result = _validator.Validate(reference, PaymentReferenceFormat.LocalFinland);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Code.Should().Be(ValidationErrorCode.InvalidFormat);
    }
}
