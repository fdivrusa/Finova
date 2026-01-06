using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Slovenia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovenia.Validators;

public class SloveniaPaymentReferenceValidatorTests
{
    private readonly SloveniaPaymentReferenceValidator _validator;

    public SloveniaPaymentReferenceValidatorTests()
    {
        _validator = new SloveniaPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("SI1212345672")] // Valid SI12 reference
    [InlineData("SI1210003")] // Valid SI12 reference
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
    [InlineData("SI1212345600")] // Invalid check digit
    [InlineData("SI12")] // Too short
    [InlineData("123456")] // Missing prefix
    [InlineData("XX1212345672")] // Wrong prefix
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalSloveniaFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "SI1212345672";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SI1212345672", "123456", PaymentReferenceFormat.LocalSlovenia)]
    [InlineData("SI1210003", "100", PaymentReferenceFormat.LocalSlovenia)]
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
    [InlineData("SI1212345600")] // Invalid check digit
    public void Parse_WithInvalidReference_ReturnsNull(string? reference)
    {
        // Act
        var result = _validator.Parse(reference);

        // Assert
        result.Should().BeNull();
    }
}
