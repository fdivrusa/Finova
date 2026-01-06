using Finova.Core.Enterprise;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Enterprise;

public class EnterpriseNumberNormalizerTests
{
    [Theory]
    [InlineData("BE0123456789", "BE", "0123456789")]
    [InlineData("be0123456789", "BE", "0123456789")]
    [InlineData("FR123456789", "FR", "123456789")]
    [InlineData("123456789", "BE", "123456789")] // Prefix missing, return sanitized
    [InlineData("BE 0123.456.789", "BE", "0123456789")] // Formatted
    public void Normalize_RemovesPrefixAndSanitizes(string input, string country, string expected)
    {
        EnterpriseNumberNormalizer.Normalize(input, country).Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "BE", null)]
    [InlineData("", "BE", "")]
    [InlineData("   ", "BE", "   ")]
    public void Normalize_HandlesNullOrEmpty(string? input, string country, string? expected)
    {
        EnterpriseNumberNormalizer.Normalize(input, country).Should().Be(expected);
    }
}
