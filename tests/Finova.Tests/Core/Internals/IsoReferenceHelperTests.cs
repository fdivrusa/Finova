using Finova.Core.PaymentReference.Internals;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Internals;

public class IsoReferenceHelperTests
{
    [Fact]
    public void Generate_WithValidReference_ReturnsValidIsoFormat()
    {
        // Arrange
        var rawReference = "1234567890";

        // Act
        var result = IsoReferenceHelper.Generate(rawReference);

        // Assert
        result.Should().StartWith("RF");
        result.Length.Should().BeGreaterThan(4);
        // The result should have RF + 2 check digits + the reference
        result.Should().MatchRegex(@"^RF\d{2}.*");
    }

    [Fact]
    public void Generate_WithValidReference_PassesValidation()
    {
        // Arrange
        var rawReference = "1234567890";

        // Act
        var result = IsoReferenceHelper.Generate(rawReference);

        // Assert - Generated reference should be valid
        IsoReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("INVOICE123")]
    [InlineData("ABC")]
    [InlineData("123XYZ456")]
    public void Generate_WithAlphanumericReferences_ReturnsValidFormat(string rawReference)
    {
        // Act
        var result = IsoReferenceHelper.Generate(rawReference);

        // Assert
        result.Should().StartWith("RF");
        result.Should().Contain(rawReference.ToUpperInvariant());
        IsoReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_WithShortReferences_ReturnsValidFormat()
    {
        // Arrange & Act  
        // Note: Very short references (1-2 chars) may not validate due to minimum length requirements
        var result3 = IsoReferenceHelper.Generate("123");
        var result4 = IsoReferenceHelper.Generate("1234");
        var result5 = IsoReferenceHelper.Generate("12345");

        // Assert
        result3.Should().StartWith("RF");
        result4.Should().StartWith("RF");
        result5.Should().StartWith("RF");

        // These generated references should be valid
        IsoReferenceValidator.Validate(result3).IsValid.Should().BeTrue();
        IsoReferenceValidator.Validate(result4).IsValid.Should().BeTrue();
        IsoReferenceValidator.Validate(result5).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Generate_WithNullOrWhiteSpace_ThrowsArgumentException(string? rawReference)
    {
        // Act
        Action act = () => IsoReferenceHelper.Generate(rawReference!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Raw reference cannot be empty for ISO generation.*")
            .And.ParamName.Should().Be("rawReference");
    }

    [Fact]
    public void Generate_WithLeadingAndTrailingWhitespace_TrimsInput()
    {
        // Arrange
        var rawReference = "  TEST123  ";

        // Act
        var result = IsoReferenceHelper.Generate(rawReference);

        // Assert
        result.Should().Contain("TEST123");
        result.Should().NotContain(" TEST123");
        IsoReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_WithLowercaseReference_ConvertsToUppercase()
    {
        // Arrange
        var rawReference = "invoice123";

        // Act
        var result = IsoReferenceHelper.Generate(rawReference);

        // Assert
        result.Should().Contain("INVOICE123");
        result.Should().NotContain("invoice123");
        IsoReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_WithSameInput_ReturnsSameOutput()
    {
        // Arrange
        var rawReference = "CONSISTENT";

        // Act
        var result1 = IsoReferenceHelper.Generate(rawReference);
        var result2 = IsoReferenceHelper.Generate(rawReference);

        // Assert
        result1.Should().Be(result2);
    }

    [Theory]
    [InlineData("12345678901234567890", "RF")] // 20 chars
    [InlineData("1234567890123456789012345", "RF")] // 25 chars - max for ISO
    public void Generate_WithLongReferences_ReturnsValidFormat(string rawReference, string expectedPrefix)
    {
        // Act
        var result = IsoReferenceHelper.Generate(rawReference);

        // Assert
        result.Should().StartWith(expectedPrefix);
        IsoReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_WithNumericReference_CalculatesCorrectCheckDigits()
    {
        // Arrange
        var rawReference = "539007547034";

        // Act
        var result = IsoReferenceHelper.Generate(rawReference);

        // Assert
        result.Should().StartWith("RF");
        // The check digits should make the whole reference valid
        IsoReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }
}
