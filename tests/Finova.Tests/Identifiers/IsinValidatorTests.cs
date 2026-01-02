using Finova.Core.Common;
using Finova.Core.Identifiers;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Identifiers;

public class IsinValidatorTests
{
    private readonly IsinValidator _validator = new();

    #region Validate Tests

    [Theory]
    [InlineData("US0378331005")] // Apple Inc.
    [InlineData("US5949181045")] // Microsoft
    [InlineData("US88160R1014")] // Tesla
    [InlineData("GB0002634946")] // BAE Systems (UK)
    [InlineData("DE0007164600")] // SAP AG (Germany)
    [InlineData("FR0000131104")] // BNP Paribas (France)
    [InlineData("JP3633400001")] // Toyota Motor (Japan)
    [InlineData("CH0012032048")] // Roche (Switzerland)
    [InlineData("NL0000009165")] // Heineken (Netherlands)
    [InlineData("AU000000BHP4")] // BHP Group (Australia)
    public void Validate_ValidIsin_ReturnsSuccess(string isin)
    {
        // Act
        var result = IsinValidator.Validate(isin);

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
        var result = IsinValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("US037833100")] // Too short (11)
    [InlineData("US03783310055")] // Too long (13)
    [InlineData("US0378")] // Way too short
    public void Validate_InvalidLength_ReturnsInvalidLength(string isin)
    {
        // Act
        var result = IsinValidator.Validate(isin);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("12US03783310")] // Starts with numbers
    [InlineData("US037833100A")] // Check digit must be numeric
    public void Validate_InvalidFormat_ReturnsInvalidFormat(string isin)
    {
        // Act
        var result = IsinValidator.Validate(isin);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("US0378331006")] // Wrong check digit (5 -> 6)
    [InlineData("US0378331004")] // Wrong check digit (5 -> 4)
    [InlineData("GB0002634940")] // Wrong check digit (6 -> 0)
    public void Validate_InvalidChecksum_ReturnsInvalidChecksum(string isin)
    {
        // Act
        var result = IsinValidator.Validate(isin);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_WithLowercaseIsin_ReturnsSuccess()
    {
        // Arrange
        var isin = "us0378331005";

        // Act
        var result = IsinValidator.Validate(isin);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithWhitespace_ReturnsSuccess()
    {
        // Arrange
        var isin = "  US0378331005  ";

        // Act
        var result = IsinValidator.Validate(isin);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Parse Tests

    [Fact]
    public void Parse_ValidIsin_ReturnsCorrectDetails()
    {
        // Arrange
        var isin = "US0378331005";

        // Act
        var details = IsinValidator.Parse(isin);

        // Assert
        details.Should().NotBeNull();
        details!.Isin.Should().Be("US0378331005");
        details.CountryCode.Should().Be("US");
        details.Nsin.Should().Be("037833100");
        details.CheckDigit.Should().Be('5');
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_InvalidIsin_ReturnsDetailsWithIsValidFalse()
    {
        // Arrange
        var isin = "US0378331006"; // Wrong check digit

        // Act
        var details = IsinValidator.Parse(isin);

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_NullInput_ReturnsNull()
    {
        // Act
        var details = IsinValidator.Parse(null);

        // Assert
        details.Should().BeNull();
    }

    [Fact]
    public void Parse_TooShortInput_ReturnsNull()
    {
        // Act
        var details = IsinValidator.Parse("US12345");

        // Assert
        details.Should().BeNull();
    }

    #endregion

    #region CalculateCheckDigit Tests

    [Theory]
    [InlineData("US037833100", '5')] // Apple
    [InlineData("GB000263494", '6')] // BAE Systems
    [InlineData("DE000716460", '0')] // SAP
    public void CalculateCheckDigit_ValidBase_ReturnsCorrectCheckDigit(string isinBase, char expectedCheckDigit)
    {
        // Act
        var checkDigit = IsinValidator.CalculateCheckDigit(isinBase);

        // Assert
        checkDigit.Should().Be(expectedCheckDigit);
    }

    [Fact]
    public void CalculateCheckDigit_InvalidLength_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => IsinValidator.CalculateCheckDigit("US12345"));
    }

    #endregion

    #region Generate Tests

    [Fact]
    public void Generate_ValidInputs_ReturnsValidIsin()
    {
        // Arrange
        var countryCode = "US";
        var nsin = "037833100";

        // Act
        var isin = IsinValidator.Generate(countryCode, nsin);

        // Assert
        isin.Should().Be("US0378331005");
        IsinValidator.Validate(isin).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_InvalidCountryCode_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => IsinValidator.Generate("USA", "037833100"));
    }

    [Fact]
    public void Generate_InvalidNsin_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => IsinValidator.Generate("US", "12345")); // Too short
    }

    #endregion

    #region Interface Tests

    [Fact]
    public void Interface_Validate_WorksCorrectly()
    {
        // Arrange
        IIsinValidator validator = _validator;

        // Act
        var result = validator.Validate("US0378331005");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Interface_Parse_WorksCorrectly()
    {
        // Arrange
        IIsinValidator validator = _validator;

        // Act
        var details = validator.Parse("US0378331005");

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeTrue();
    }

    #endregion
}
