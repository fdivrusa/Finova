using Finova.Germany.Models;
using Finova.Germany.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Germany.Services
{
    public class GermanyIbanParserTests
    {
        private readonly GermanyIbanParser _parser = GermanyIbanParser.CreateDefault();

        [Fact]
        public void CountryCode_ReturnsDE() => _parser.CountryCode.Should().Be("DE");

        [Fact]
        public void ParseIban_ExtractsAllComponents()
        {
            var result = _parser.ParseIban("DE89370400440532013000") as GermanyIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("DE89370400440532013000");
            result.CountryCode.Should().Be("DE");
            result.CheckDigits.Should().Be("89");
            result.Bankleitzahl.Should().Be("37040044");
            result.Kontonummer.Should().Be("0532013000");
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("DE00370400440532013000")] // Invalid
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
            => _parser.ParseIban(iban).Should().BeNull();

        [Fact]
        public void CreateDefault_ReturnsValidParser()
            => GermanyIbanParser.CreateDefault().CountryCode.Should().Be("DE");
    }
}