using Finova.Core.PaymentReference;
using Finova.Extensions;
using Finova.Validators;
using Finova.Generators;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Finova.Tests.Generators;

public class PaymentReferenceGeneratorTests
{
    private readonly PaymentReferenceGenerator _generator;

    public PaymentReferenceGeneratorTests()
    {
        _generator = new PaymentReferenceGenerator();
    }

    #region Generate Tests - All Formats

    [Theory]
    [InlineData("123456789")]
    [InlineData("INVOICE2023")]
    [InlineData("AB12CD34")]
    public void Generate_WithIsoRfFormat_CreatesValidReference(string input)
    {
        // Act
        var result = _generator.Generate(input, PaymentReferenceFormat.IsoRf);

        // Assert
        result.Should().StartWith("RF");
        PaymentReferenceValidator.Validate(result).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("0")]
    [InlineData("123456")]
    [InlineData("1234567890")]
    public void Generate_WithLocalBelgianFormat_CreatesValidOgm(string input)
    {
        // Act
        var result = _generator.Generate(input, PaymentReferenceFormat.LocalBelgian);

        // Assert
        result.Should().StartWith("+++");
        result.Should().EndWith("+++");
        result.Should().MatchRegex(@"\+\+\+\d{3}/\d{4}/\d{5}\+\+\+"); // Format: +++XXX/XXXX/XXXXX+++
        PaymentReferenceValidator.Validate(result, PaymentReferenceFormat.LocalBelgian).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345")]
    [InlineData("1234567890")]
    public void Generate_WithLocalFinlandFormat_CreatesValidViitenumero(string input)
    {
        // Act
        var result = _generator.Generate(input, PaymentReferenceFormat.LocalFinland);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Length.Should().Be(input.Length + 1); // Original + 1 check digit
        PaymentReferenceValidator.Validate(result, PaymentReferenceFormat.LocalFinland).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("123456789")]
    public void Generate_WithLocalNorwayFormat_CreatesValidKid(string input)
    {
        // Act
        var result = _generator.Generate(input, PaymentReferenceFormat.LocalNorway);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Length.Should().Be(input.Length + 1); // Original + 1 check digit (Luhn)
        PaymentReferenceValidator.Validate(result, PaymentReferenceFormat.LocalNorway).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123456")]
    public void Generate_WithLocalSwedenFormat_CreatesValidOcr(string input)
    {
        // Act
        var result = _generator.Generate(input, PaymentReferenceFormat.LocalSweden);

        // Assert
        result.Should().NotBeNullOrEmpty();
        PaymentReferenceValidator.Validate(result, PaymentReferenceFormat.LocalSweden).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("12345678901234567890123456")] // 26 digits
    public void Generate_WithLocalSwitzerlandFormat_CreatesValidQrReference(string input)
    {
        // Act
        var result = _generator.Generate(input, PaymentReferenceFormat.LocalSwitzerland);

        // Assert
        result.Should().HaveLength(27); // 26 digits + 1 check digit
        PaymentReferenceValidator.Validate(result, PaymentReferenceFormat.LocalSwitzerland).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123456789012")]
    public void Generate_WithLocalSloveniaFormat_CreatesValidSi12(string input)
    {
        // Act
        var result = _generator.Generate(input, PaymentReferenceFormat.LocalSlovenia);

        // Assert
        result.Should().StartWith("SI12");
        PaymentReferenceValidator.Validate(result, PaymentReferenceFormat.LocalSlovenia).IsValid.Should().BeTrue();
    }

    #endregion

    #region IsValid Tests

    [Theory]
    [InlineData("RF712348231", true)] // ISO RF
    [InlineData("INVALID", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid_WithVariousInputs_ReturnsExpected(string? input, bool expected)
    {
        // Act
        var result = PaymentReferenceValidator.Validate(input!).IsValid;

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void IsValid_WithIsoRfReference_ReturnsTrue()
    {
        // Arrange
        var reference = _generator.Generate("TEST123", PaymentReferenceFormat.IsoRf);

        // Act
        var result = PaymentReferenceValidator.Validate(reference).IsValid;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithBelgianOgm_ReturnsTrue()
    {
        // Arrange
        var reference = _generator.Generate("12345", PaymentReferenceFormat.LocalBelgian);

        // Act
        var result = PaymentReferenceValidator.Validate(reference, PaymentReferenceFormat.LocalBelgian).IsValid;

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region DI Integration Tests

    [Fact]
    public void DiRegistration_ResolvesPaymentReferenceGenerator()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFinova();
        var provider = services.BuildServiceProvider();

        // Act
        var generator = provider.GetService<IPaymentReferenceGenerator>();

        // Assert
        generator.Should().NotBeNull();
        generator.Should().BeOfType<PaymentReferenceGenerator>();
    }

    [Fact]
    public void DiRegistration_GeneratorSupportsAllFormats()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFinova();
        var provider = services.BuildServiceProvider();
        var generator = provider.GetRequiredService<IPaymentReferenceGenerator>();

        // Act & Assert - Should not throw for any format
        generator.Generate("12345", PaymentReferenceFormat.IsoRf).Should().StartWith("RF");
        generator.Generate("12345", PaymentReferenceFormat.LocalBelgian).Should().Contain("+++");
        generator.Generate("123", PaymentReferenceFormat.LocalFinland).Should().NotBeNullOrEmpty();
        generator.Generate("12345", PaymentReferenceFormat.LocalNorway).Should().NotBeNullOrEmpty();
        generator.Generate("123456", PaymentReferenceFormat.LocalSweden).Should().NotBeNullOrEmpty();
        generator.Generate("12345678901234567890123456", PaymentReferenceFormat.LocalSwitzerland).Should().HaveLength(27);
        generator.Generate("123456789012", PaymentReferenceFormat.LocalSlovenia).Should().StartWith("SI12");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Generate_WithUnsupportedFormat_ThrowsNotSupportedException()
    {
        // Arrange
        var invalidFormat = (PaymentReferenceFormat)999;

        // Act
        Action act = () => _generator.Generate("12345", invalidFormat);

        // Assert
        act.Should().Throw<NotSupportedException>()
           .WithMessage("*999*not supported*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_WithNullOrEmpty_ReturnsFalse(string? input)
    {
        // Act
        var result = PaymentReferenceValidator.Validate(input!).IsValid;

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}
