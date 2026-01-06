using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Norway.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Norway.Validators;

public class NorwayPaymentReferenceValidatorTests
{
    private readonly NorwayPaymentReferenceValidator _validator;

    public NorwayPaymentReferenceValidatorTests()
    {
        _validator = new NorwayPaymentReferenceValidator();
    }

    [Theory]
    [InlineData("123456782")] // Valid KID (Mod10)
    [InlineData("2345676")] // Valid KID
    [InlineData("123456785")] // Valid KID (Mod11)
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
    [InlineData("123456780")] // Invalid check digit
    [InlineData("12")] // Too short
    [InlineData("12345678901234567890123456")] // Too long
    public void Validate_WithInvalidReference_ReturnsFailure(string? reference)
    {
        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithLocalNorwayFormat_ReturnsSuccess()
    {
        // Arrange
        var reference = "123456782";

        // Act
        var result = _validator.Validate(reference);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123456782", "12345678", PaymentReferenceFormat.LocalNorway)]
    [InlineData("2345676", "234567", PaymentReferenceFormat.LocalNorway)]
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
    [InlineData("123456780")] // Invalid check digit
    public void Parse_WithInvalidReference_ReturnsNull(string? reference)
    {
        // Act
        var result = _validator.Parse(reference);

        // Assert
        result.Should().BeNull();
    }
}
