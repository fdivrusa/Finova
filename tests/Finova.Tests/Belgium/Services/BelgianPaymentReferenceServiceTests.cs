using Finova.Belgium.Services;
using Finova.Core.PaymentReference.Internals;
using Finova.Core.PaymentReference;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Belgium.Services;

public class BelgianPaymentReferenceServiceTests
{
    private readonly BelgiumPaymentReferenceService _service;

    public BelgianPaymentReferenceServiceTests()
    {
        _service = new BelgiumPaymentReferenceService();
    }

    [Fact]
    public void CountryCode_ReturnsCorrectValue()
    {
        // Act
        var result = _service.CountryCode;

        // Assert
        result.Should().Be("BE");
    }

    #region OGM Format Tests

    [Theory]
    [InlineData("1", "+++000/0000/00101+++")]
    [InlineData("123", "+++000/0000/12326+++")]
    [InlineData("1234567890", "+++123/4567/89002+++")]
    public void Generate_WithDomesticFormat_ReturnsValidOgm(string rawReference, string expected)
    {
        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Generate_WithDomesticFormat_CreatesValidatableReference()
    {
        // Arrange
        var rawReference = "123456";

        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        result.Should().MatchRegex(@"^\+\+\+\d{3}/\d{4}/\d{5}\+\+\+$");
        BelgiumPaymentReferenceService.ValidateStatic(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_WithNumericStringContainingNonDigits_ExtractsDigitsOnly()
    {
        // Arrange
        var rawReference = "INV-1234-ABC";

        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        // Should extract "1234" from the string
        result.Should().MatchRegex(@"^\+\+\+\d{3}/\d{4}/\d{5}\+\+\+$");
        BelgiumPaymentReferenceService.ValidateStatic(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_WithOgmExceedingTenDigits_ThrowsArgumentException()
    {
        // Arrange
        var rawReference = "12345678901"; // 11 digits

        // Act
        Action act = () => _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("OGM reference data cannot exceed 10 digits.");
    }

    [Fact]
    public void Generate_WithEmptyString_GeneratesValidOgm()
    {
        // Arrange
        var rawReference = "";

        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        // Should generate OGM with all zeros padded
        result.Should().MatchRegex(@"^\+\+\+\d{3}/\d{4}/\d{5}\+\+\+$");
        BelgiumPaymentReferenceService.ValidateStatic(result).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("+++000/0000/00101+++")]
    [InlineData("+++000/0000/12326+++")]
    [InlineData("+++123/4567/89002+++")]
    public void IsValid_WithValidOgm_ReturnsTrue(string ogm)
    {
        // Act
        var result = BelgiumPaymentReferenceService.ValidateStatic(ogm);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("+++000/0000/00196+++")] // Wrong check digit
    [InlineData("+++123/4567/89036+++")] // Wrong check digit
    [InlineData("+++000/0000/01267+++")] // Wrong check digit
    public void IsValid_WithInvalidOgmCheckDigit_ReturnsFalse(string ogm)
    {
        // Act
        var result = BelgiumPaymentReferenceService.ValidateStatic(ogm);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("000000000101")] // 12 digits with formatting removed
    [InlineData("000/0000/00101")] // Partial formatting (11 chars + 2 slashes = 13 total)
    public void IsValid_WithOgmVariousFormats_ValidatesCorrectly(string ogm)
    {
        // Act
        var result = BelgiumPaymentReferenceService.ValidateStatic(ogm);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123")] // Too short
    [InlineData("12345678901234")] // Too long
    [InlineData("+++123/4567/890+++")] // Only 11 digits
    public void IsValid_WithInvalidOgmLength_ReturnsFalse(string ogm)
    {
        // Act
        var result = BelgiumPaymentReferenceService.ValidateStatic(ogm);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region ISO RF Format Tests

    [Fact]
    public void Generate_WithIsoRfFormat_ReturnsValidIsoReference()
    {
        // Arrange
        var rawReference = "INVOICE12345";

        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.IsoRf);

        // Assert
        result.Should().StartWith("RF");
        result.Should().MatchRegex(@"^RF\d{2}.*");
        IsoPaymentReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("ABC123")]
    [InlineData("CUSTOMER999")]
    public void Generate_WithIsoRfFormat_CreatesValidatableReference(string rawReference)
    {
        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.IsoRf);

        // Assert
        result.Should().StartWith("RF");
        IsoPaymentReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithValidIsoReference_ReturnsTrue()
    {
        // Arrange - Generate valid references
        var reference1 = IsoReferenceHelper.Generate("539007547034");
        var reference2 = reference1.Insert(4, " ").Insert(9, " ").Insert(14, " "); // Add spacing

        // Act
        var result1 = IsoPaymentReferenceValidator.Validate(reference1);
        var result2 = IsoPaymentReferenceValidator.Validate(reference2);

        // Assert
        result1.IsValid.Should().BeTrue();
        result2.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RF19539007547034")] // Wrong check digits
    [InlineData("RF00539007547034")] // Wrong check digits
    public void IsValid_WithInvalidIsoReference_ReturnsFalse(string reference)
    {
        // Act
        var result = IsoPaymentReferenceValidator.Validate(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Format Support Tests

    [Fact]
    public void Generate_WithDefaultFormat_UsesDomestic()
    {
        // Arrange
        var rawReference = "12345";

        // Act
        var result = _service.Generate(rawReference);

        // Assert
        result.Should().MatchRegex(@"^\+\+\+\d{3}/\d{4}/\d{5}\+\+\+$");
    }

    [Fact]
    public void Generate_WithUnsupportedFormat_ThrowsNotSupportedException()
    {
        // Arrange
        var rawReference = "12345";
        var unsupportedFormat = (PaymentReferenceFormat)999;

        // Act
        Action act = () => _service.Generate(rawReference, unsupportedFormat);

        // Assert
        act.Should().Throw<NotSupportedException>()
            .WithMessage("Format * is not supported by BE");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Generate_WithSpecialCharactersInOgm_ExtractsDigitsOnly()
    {
        // Arrange
        var rawReference = "ABC-123/XYZ-456";

        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        result.Should().MatchRegex(@"^\+\+\+\d{3}/\d{4}/\d{5}\+\+\+$");
        BelgiumPaymentReferenceService.ValidateStatic(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithWhitespaceOnlyOgm_ReturnsFalse()
    {
        // Arrange
        var reference = "   ";

        // Act
        var result = BelgiumPaymentReferenceService.ValidateStatic(reference);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_WithNullOrEmptyString_ReturnsFalse(string? reference)
    {
        // Act
        var result = BelgiumPaymentReferenceService.ValidateStatic(reference!);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Generate_MultipleTimes_WithSameInput_ReturnsSameOutput()
    {
        // Arrange
        var rawReference = "CONSISTENT";

        // Act
        var result1 = _service.Generate(rawReference, PaymentReferenceFormat.IsoRf);
        var result2 = _service.Generate(rawReference, PaymentReferenceFormat.IsoRf);

        // Assert
        result1.Should().Be(result2);
    }

    [Fact]
    public void Generate_WithZeroValue_GeneratesValidOgm()
    {
        // Arrange
        var rawReference = "0";

        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        result.Should().MatchRegex(@"^\+\+\+\d{3}/\d{4}/\d{5}\+\+\+$");
        BelgiumPaymentReferenceService.ValidateStatic(result).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Generate_WithMaxLengthInput_GeneratesValidOgm()
    {
        // Arrange
        var rawReference = "9999999999"; // 10 digits - max allowed

        // Act
        var result = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);

        // Assert
        result.Should().MatchRegex(@"^\+\+\+\d{3}/\d{4}/\d{5}\+\+\+$");
        BelgiumPaymentReferenceService.ValidateStatic(result).IsValid.Should().BeTrue();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void GenerateAndValidate_RoundTrip_Domestic_WorksCorrectly()
    {
        // Arrange
        var rawReference = "123456789";

        // Act
        var generated = _service.Generate(rawReference, PaymentReferenceFormat.LocalBelgian);
        var isValid = BelgiumPaymentReferenceService.ValidateStatic(generated).IsValid;

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void GenerateAndValidate_RoundTrip_IsoRf_WorksCorrectly()
    {
        // Arrange
        var rawReference = "INVOICE2024";

        // Act
        var generated = _service.Generate(rawReference, PaymentReferenceFormat.IsoRf);
        var isValid = IsoPaymentReferenceValidator.Validate(generated).IsValid;

        // Assert
        isValid.Should().BeTrue();
    }

    #endregion
}
