using Finova.Core.Common;
using Finova.Core.Identifiers;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Identifiers;

public class SedolValidatorTests
{
    private readonly SedolValidator _validator = new();

    #region Validate Tests

    [Theory]
    [InlineData("0263494")] // BAE Systems
    [InlineData("B0YBKJ7")] // Tesco
    [InlineData("0540528")] // HSBC
    [InlineData("B0YQ5W0")] // Vodafone
    [InlineData("3134865")] // GlaxoSmithKline
    [InlineData("B1YW440")] // Rio Tinto
    public void Validate_ValidSedol_ReturnsSuccess(string sedol)
    {
        // Act
        var result = SedolValidator.Validate(sedol);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyInput_ReturnsInvalidInput(string? input)
    {
        // Act
        var result = SedolValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("026349")] // Too short (6)
    [InlineData("02634946")] // Too long (8)
    [InlineData("026")] // Way too short
    public void Validate_InvalidLength_ReturnsInvalidLength(string sedol)
    {
        // Act
        var result = SedolValidator.Validate(sedol);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("A263494")] // Contains vowel 'A'
    [InlineData("E263494")] // Contains vowel 'E'
    [InlineData("I263494")] // Contains vowel 'I'
    [InlineData("O263494")] // Contains vowel 'O'
    [InlineData("U263494")] // Contains vowel 'U'
    [InlineData("026349!")] // Invalid character
    public void Validate_InvalidFormat_ReturnsInvalidFormat(string sedol)
    {
        // Act
        var result = SedolValidator.Validate(sedol);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("0263495")] // Wrong check digit (4 -> 5)
    [InlineData("0263490")] // Wrong check digit (4 -> 0)
    [InlineData("B0YBKJ0")] // Wrong check digit (7 -> 0)
    public void Validate_InvalidChecksum_ReturnsInvalidChecksum(string sedol)
    {
        // Act
        var result = SedolValidator.Validate(sedol);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_WithLowercaseSedol_ReturnsSuccess()
    {
        // Arrange
        var sedol = "b0ybkj7";

        // Act
        var result = SedolValidator.Validate(sedol);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithWhitespace_ReturnsSuccess()
    {
        // Arrange
        var sedol = "  0263494  ";

        // Act
        var result = SedolValidator.Validate(sedol);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Parse Tests

    [Fact]
    public void Parse_ValidSedol_ReturnsCorrectDetails()
    {
        // Arrange
        var sedol = "0263494";

        // Act
        var details = SedolValidator.Parse(sedol);

        // Assert
        details.Should().NotBeNull();
        details!.Sedol.Should().Be("0263494");
        details.BaseCode.Should().Be("026349");
        details.CheckDigit.Should().Be('4');
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_SedolWithLetters_ReturnsCorrectDetails()
    {
        // Arrange
        var sedol = "B0YBKJ7";

        // Act
        var details = SedolValidator.Parse(sedol);

        // Assert
        details.Should().NotBeNull();
        details!.Sedol.Should().Be("B0YBKJ7");
        details.BaseCode.Should().Be("B0YBKJ");
        details.CheckDigit.Should().Be('7');
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_InvalidSedol_ReturnsDetailsWithIsValidFalse()
    {
        // Arrange
        var sedol = "0263495"; // Wrong check digit

        // Act
        var details = SedolValidator.Parse(sedol);

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_NullInput_ReturnsNull()
    {
        // Act
        var details = SedolValidator.Parse(null);

        // Assert
        details.Should().BeNull();
    }

    [Fact]
    public void Parse_TooShortInput_ReturnsNull()
    {
        // Act
        var details = SedolValidator.Parse("1234");

        // Assert
        details.Should().BeNull();
    }

    #endregion

    #region CalculateCheckDigit Tests

    [Theory]
    [InlineData("026349", '4')] // BAE Systems
    [InlineData("B0YBKJ", '7')] // Tesco
    [InlineData("054052", '8')] // HSBC
    public void CalculateCheckDigit_ValidBase_ReturnsCorrectCheckDigit(string sedolBase, char expectedCheckDigit)
    {
        // Act
        var checkDigit = SedolValidator.CalculateCheckDigit(sedolBase);

        // Assert
        checkDigit.Should().Be(expectedCheckDigit);
    }

    [Fact]
    public void CalculateCheckDigit_InvalidLength_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => SedolValidator.CalculateCheckDigit("12345"));
    }

    [Fact]
    public void CalculateCheckDigit_ContainsVowel_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => SedolValidator.CalculateCheckDigit("A12345"));
    }

    #endregion

    #region Generate Tests

    [Fact]
    public void Generate_ValidBase_ReturnsValidSedol()
    {
        // Arrange
        var sedolBase = "026349";

        // Act
        var sedol = SedolValidator.Generate(sedolBase);

        // Assert
        sedol.Should().Be("0263494");
        SedolValidator.Validate(sedol).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_InvalidLength_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => SedolValidator.Generate("12345"));
    }

    #endregion

    #region Interface Tests

    [Fact]
    public void Interface_Validate_WorksCorrectly()
    {
        // Arrange
        ISedolValidator validator = _validator;

        // Act
        var result = validator.Validate("0263494");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Interface_Parse_WorksCorrectly()
    {
        // Arrange
        ISedolValidator validator = _validator;

        // Act
        var details = validator.Parse("0263494");

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeTrue();
    }

    #endregion
}
