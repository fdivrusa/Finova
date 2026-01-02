using Finova.Core.Common;
using Finova.Core.Identifiers;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Identifiers;

public class CusipValidatorTests
{
    private readonly CusipValidator _validator = new();

    #region Validate Tests

    [Theory]
    [InlineData("037833100")] // Apple Inc.
    [InlineData("594918104")] // Microsoft
    [InlineData("88160R101")] // Tesla
    [InlineData("17275R102")] // Cisco Systems
    [InlineData("68389X105")] // Oracle
    [InlineData("30303M102")] // Meta (Facebook)
    [InlineData("023135106")] // Amazon
    public void Validate_ValidCusip_ReturnsSuccess(string cusip)
    {
        // Act
        var result = CusipValidator.Validate(cusip);

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
        var result = CusipValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("03783310")] // Too short (8)
    [InlineData("0378331005")] // Too long (10)
    [InlineData("037")] // Way too short
    public void Validate_InvalidLength_ReturnsInvalidLength(string cusip)
    {
        // Act
        var result = CusipValidator.Validate(cusip);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("03783310X")] // Check digit must be numeric
    [InlineData("0378331!0")] // Invalid character
    public void Validate_InvalidFormat_ReturnsInvalidFormat(string cusip)
    {
        // Act
        var result = CusipValidator.Validate(cusip);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("037833101")] // Wrong check digit (0 -> 1)
    [InlineData("037833109")] // Wrong check digit (0 -> 9)
    [InlineData("594918105")] // Wrong check digit (4 -> 5)
    public void Validate_InvalidChecksum_ReturnsInvalidChecksum(string cusip)
    {
        // Act
        var result = CusipValidator.Validate(cusip);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_WithLowercaseCusip_ReturnsSuccess()
    {
        // Arrange
        var cusip = "88160r101";

        // Act
        var result = CusipValidator.Validate(cusip);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithWhitespace_ReturnsSuccess()
    {
        // Arrange
        var cusip = "  037833100  ";

        // Act
        var result = CusipValidator.Validate(cusip);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Parse Tests

    [Fact]
    public void Parse_ValidCusip_ReturnsCorrectDetails()
    {
        // Arrange
        var cusip = "037833100";

        // Act
        var details = CusipValidator.Parse(cusip);

        // Assert
        details.Should().NotBeNull();
        details!.Cusip.Should().Be("037833100");
        details.IssuerNumber.Should().Be("037833");
        details.IssueNumber.Should().Be("10");
        details.CheckDigit.Should().Be('0');
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_CusipWithLetter_ReturnsCorrectDetails()
    {
        // Arrange
        var cusip = "88160R101"; // Tesla

        // Act
        var details = CusipValidator.Parse(cusip);

        // Assert
        details.Should().NotBeNull();
        details!.Cusip.Should().Be("88160R101");
        details.IssuerNumber.Should().Be("88160R");
        details.IssueNumber.Should().Be("10");
        details.CheckDigit.Should().Be('1');
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_InvalidCusip_ReturnsDetailsWithIsValidFalse()
    {
        // Arrange
        var cusip = "037833101"; // Wrong check digit

        // Act
        var details = CusipValidator.Parse(cusip);

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_NullInput_ReturnsNull()
    {
        // Act
        var details = CusipValidator.Parse(null);

        // Assert
        details.Should().BeNull();
    }

    [Fact]
    public void Parse_TooShortInput_ReturnsNull()
    {
        // Act
        var details = CusipValidator.Parse("12345");

        // Assert
        details.Should().BeNull();
    }

    #endregion

    #region CalculateCheckDigit Tests

    [Theory]
    [InlineData("03783310", '0')] // Apple
    [InlineData("59491810", '4')] // Microsoft
    [InlineData("88160R10", '1')] // Tesla
    public void CalculateCheckDigit_ValidBase_ReturnsCorrectCheckDigit(string cusipBase, char expectedCheckDigit)
    {
        // Act
        var checkDigit = CusipValidator.CalculateCheckDigit(cusipBase);

        // Assert
        checkDigit.Should().Be(expectedCheckDigit);
    }

    [Fact]
    public void CalculateCheckDigit_InvalidLength_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CusipValidator.CalculateCheckDigit("12345"));
    }

    #endregion

    #region Generate Tests

    [Fact]
    public void Generate_ValidInputs_ReturnsValidCusip()
    {
        // Arrange
        var issuerNumber = "037833";
        var issueNumber = "10";

        // Act
        var cusip = CusipValidator.Generate(issuerNumber, issueNumber);

        // Assert
        cusip.Should().Be("037833100");
        CusipValidator.Validate(cusip).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_InvalidIssuerNumber_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CusipValidator.Generate("12345", "10"));
    }

    [Fact]
    public void Generate_InvalidIssueNumber_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CusipValidator.Generate("037833", "1")); // Too short
    }

    #endregion

    #region Interface Tests

    [Fact]
    public void Interface_Validate_WorksCorrectly()
    {
        // Arrange
        ICusipValidator validator = _validator;

        // Act
        var result = validator.Validate("037833100");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Interface_Parse_WorksCorrectly()
    {
        // Arrange
        ICusipValidator validator = _validator;

        // Act
        var details = validator.Parse("037833100");

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeTrue();
    }

    #endregion
}
