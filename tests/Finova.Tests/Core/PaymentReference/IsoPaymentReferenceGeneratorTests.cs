using Finova.Core.PaymentReference;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.PaymentReference;

public class IsoPaymentReferenceGeneratorTests
{
    private readonly IsoPaymentReferenceGenerator _generator;

    public IsoPaymentReferenceGeneratorTests()
    {
        _generator = new IsoPaymentReferenceGenerator();
    }

    [Theory]
    [InlineData("123456789")]
    [InlineData("INVOICE2023")]
    [InlineData("AB12CD34")]
    public void Generate_WithValidInput_CreatesValidIsoRfReference(string input)
    {
        // Act
        var result = _generator.Generate(input);

        // Assert
        result.Should().StartWith("RF");
        result.Length.Should().BeGreaterThan(4);
    }

    [Fact]
    public void Generate_WithNullInput_ThrowsArgumentException()
    {
        // Act
        Action act = () => _generator.Generate(null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Generate_WithEmptyInput_ThrowsArgumentException()
    {
        // Act
        Action act = () => _generator.Generate(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

}
