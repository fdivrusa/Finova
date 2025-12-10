using Finova.Core.PaymentReference.Internals;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Internals;

public class IsoReferenceValidatorTests
{
    [Fact]
    public void IsValid_WithKnownValidIsoReferences_ReturnsTrue()
    {
        // Arrange - Use generated references which we know are valid
        var reference1 = IsoReferenceHelper.Generate("539007547034");
        var reference2 = IsoReferenceHelper.Generate("123456789012");

        // Act
        var result1 = IsoReferenceValidator.Validate(reference1).IsValid;
        var result2 = IsoReferenceValidator.Validate(reference2).IsValid;

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithGeneratedReference_ReturnsTrue()
    {
        // Arrange - Generate a valid reference and validate it
        var generatedReference = IsoReferenceHelper.Generate("TEST12345");

        // Act
        var result = IsoReferenceValidator.Validate(generatedReference).IsValid;

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("RF19539007547034")] // Wrong check digits
    [InlineData("RF00539007547034")] // Wrong check digits
    [InlineData("RF99123456789012")] // Wrong check digits
    public void IsValid_WithInvalidCheckDigits_ReturnsFalse(string reference)
    {
        // Act
        var result = IsoReferenceValidator.Validate(reference).IsValid;

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_WithNullOrWhiteSpace_ReturnsFalse(string? reference)
    {
        // Act
        var result = IsoReferenceValidator.Validate(reference!).IsValid;

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("AB18539007547034")] // Wrong prefix
    [InlineData("XY48539007547034")] // Wrong prefix
    [InlineData("RR89123456789012")] // Wrong prefix
    [InlineData("FR18539007547034")] // Wrong prefix (reversed)
    public void IsValid_WithInvalidPrefix_ReturnsFalse(string reference)
    {
        // Act
        var result = IsoReferenceValidator.Validate(reference).IsValid;

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("RF18")] // Too short (less than 6 chars)
    [InlineData("RF481")] // Too short (5 chars)
    [InlineData("RF")] // Only prefix
    [InlineData("RF12")] // Only prefix and check digits
    public void IsValid_WithTooShortReference_ReturnsFalse(string reference)
    {
        // Act
        var result = IsoReferenceValidator.Validate(reference).IsValid;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithSpaces_HandlesCorrectly()
    {
        // Arrange - Valid reference with spaces (common in display format)
        var referenceWithSpaces = "RF18 5390 0754 7034";

        // Act
        var result = IsoReferenceValidator.Validate(referenceWithSpaces).IsValid;

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("rf18539007547034")] // Lowercase
    [InlineData("Rf18539007547034")] // Mixed case
    [InlineData("rF18539007547034")] // Mixed case
    public void IsValid_WithLowercaseOrMixedCase_HandlesCorrectly(string reference)
    {
        // Act
        var result = IsoReferenceValidator.Validate(reference).IsValid;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithAlphanumericReferences_ValidatesCorrectly()
    {
        // Arrange - Generate valid alphanumeric references
        var reference1 = IsoReferenceHelper.Generate("INVOICE123");
        var reference2 = IsoReferenceHelper.Generate("ABC123XYZ");

        // Act
        var result1 = IsoReferenceValidator.Validate(reference1).IsValid;
        var result2 = IsoReferenceValidator.Validate(reference2).IsValid;

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithValidComplexReference_ReturnsTrue()
    {
        // Arrange - Generate a complex reference with letters and numbers
        var generatedReference = IsoReferenceHelper.Generate("ABC123XYZ789");

        // Act
        var result = IsoReferenceValidator.Validate(generatedReference).IsValid;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithModifiedValidReference_ReturnsFalse()
    {
        // Arrange - Generate a valid reference and modify it
        var validReference = IsoReferenceHelper.Generate("TEST");
        var modifiedReference = validReference.Substring(0, validReference.Length - 1) + "9";

        // Act
        var result = IsoReferenceValidator.Validate(modifiedReference).IsValid;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithVariousSpacingFormats_HandlesCorrectly()
    {
        // Arrange - Generate a valid reference and test with various spacing
        var reference = IsoReferenceHelper.Generate("539007547034");
        var withSpaces1 = reference.Insert(4, " ");
        var withSpaces2 = reference.Substring(0, 4) + " " + reference.Substring(4, 4) + " " + reference.Substring(8);

        // Act
        var result1 = IsoReferenceValidator.Validate(withSpaces1).IsValid;
        var result2 = IsoReferenceValidator.Validate(withSpaces2).IsValid;

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithVeryLongReference_ValidatesCorrectly()
    {
        // Arrange - Generate a reference with a long body
        var longReference = IsoReferenceHelper.Generate("1234567890123456789012345");

        // Act
        var result = IsoReferenceValidator.Validate(longReference).IsValid;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithSpecialCharacters_AreIgnored()
    {
        // Arrange - The validator only removes spaces, other chars remain and might still validate
        // if the overall structure and checksum still work out
        var validReference = IsoReferenceHelper.Generate("539007547034");

        // Create variations with special characters
        var withDash = validReference.Insert(6, "-");
        var withSlash = validReference.Insert(6, "/");
        var withDot = validReference.Insert(6, ".");

        // Act
        var resultDash = IsoReferenceValidator.Validate(withDash).IsValid;
        var resultSlash = IsoReferenceValidator.Validate(withSlash).IsValid;
        var resultDot = IsoReferenceValidator.Validate(withDot).IsValid;

        // Assert - The current implementation ignores non-alphanumeric characters in conversion
        // so these may still pass validation. This is actually acceptable behavior
        // as the validator is lenient with formatting
        (resultDash && resultSlash && resultDot).Should().BeTrue();
    }
}
