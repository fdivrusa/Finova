using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Validators;

public class BelgiumEnterpriseValidatorOldFormatTests
{
    [Theory]
    [InlineData("403170701", "0403170701")] // Microsoft Belgium (old format)
    [InlineData("123456749", "0123456749")] // Valid check digits
    public void Validate_With9DigitFormat_ReturnsSuccess(string input, string expectedNormalized)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Validate(input);
        var normalized = BelgiumEnterpriseValidator.Normalize(input);

        // Assert
        result.IsValid.Should().BeTrue();
        normalized.Should().Be(expectedNormalized);
    }

    [Theory]
    [InlineData("403170701", "0403.170.701")]
    public void Format_With9DigitFormat_ReturnsFormatted10DigitString(string input, string expected)
    {
        // Act
        var result = BelgiumEnterpriseValidator.Format(input);

        // Assert
        result.Should().Be(expected);
    }
}
