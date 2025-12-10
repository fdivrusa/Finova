using Finova.Core.PaymentReference.Internals;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Internals;

public class Modulo97HelperTests
{
    [Theory]
    [InlineData("123", 26)]
    [InlineData("1234", 70)]
    [InlineData("12345", 26)]
    [InlineData("123456", 72)]
    [InlineData("1234567890", 2)]
    [InlineData("97", 0)]
    [InlineData("194", 0)]
    [InlineData("291", 0)]
    public void Calculate_WithValidNumericStrings_ReturnsCorrectModulo(string input, int expectedResult)
    {
        // Act
        var result = Modulo97Helper.Calculate(input);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void Calculate_WithZero_ReturnsZero()
    {
        // Arrange
        var input = "0";

        // Act
        var result = Modulo97Helper.Calculate(input);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Calculate_WithVeryLargeNumber_ReturnsCorrectModulo()
    {
        // Arrange - A very large number that exceeds standard integer capacity
        var input = "123456789012345678901234567890";

        // Act
        var result = Modulo97Helper.Calculate(input);

        // Assert
        result.Should().BeInRange(0, 96);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Calculate_WithNullOrWhiteSpace_ThrowsArgumentException(string? input)
    {
        // Act
        Action act = () => Modulo97Helper.Calculate(input!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Input cannot be null or empty.*")
            .And.ParamName.Should().Be("numericString");
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12a34")]
    [InlineData("123.45")]
    [InlineData("12-34")]
    [InlineData("12 34")]
    public void Calculate_WithNonNumericCharacters_ThrowsArgumentException(string input)
    {
        // Act
        Action act = () => Modulo97Helper.Calculate(input);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Input must contain only digits.*");
    }

    [Fact]
    public void Calculate_WithSingleDigit_ReturnsCorrectModulo()
    {
        // Arrange
        var input = "5";

        // Act
        var result = Modulo97Helper.Calculate(input);

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void Calculate_WithLeadingZeros_HandlesCorrectly()
    {
        // Arrange
        var input = "0000123";

        // Act
        var result = Modulo97Helper.Calculate(input);

        // Assert
        result.Should().Be(26); // Same as "123"
    }

    [Theory]
    [InlineData("98", 1)]
    [InlineData("195", 1)]
    [InlineData("292", 1)]
    public void Calculate_WithMultiplesOfNinetySevenPlusOne_ReturnsOne(string input, int expectedResult)
    {
        // Act
        var result = Modulo97Helper.Calculate(input);

        // Assert
        result.Should().Be(expectedResult);
    }

    //Test for very large number that is exactly divisible by 97
    [Fact]
    public void Calculate_WithVeryLargeNumberDivisibleBy97_ReturnsZero()
    {
        // Arrange - A very large number that is exactly divisible by 97
        var input = "970000000000000000000000000000";
        // Act
        var result = Modulo97Helper.Calculate(input);
        // Assert
        result.Should().Be(0);
    }
}
