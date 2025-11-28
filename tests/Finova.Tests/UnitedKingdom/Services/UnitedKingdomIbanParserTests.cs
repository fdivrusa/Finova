using Finova.UnitedKingdom.Models;
using Finova.UnitedKingdom.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.UnitedKingdom.Services
{
    public class UnitedKingdomIbanParserTests
    {
        private readonly UnitedKingdomIbanParser _parser = UnitedKingdomIbanParser.Create();

        [Fact]
        public void CountryCode_ReturnsGB() => _parser.CountryCode.Should().Be("GB");

        [Fact]
        public void ParseIban_ExtractsAllComponents()
        {
            var result = _parser.ParseIban("GB29NWBK60161331926819") as UnitedKingdomIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("GB29NWBK60161331926819");
            result.CountryCode.Should().Be("GB");
            result.CheckDigits.Should().Be("29");
            result.BankIdentifier.Should().Be("NWBK");
            result.SortCode.Should().Be("601613");
            result.UkAccountNumber.Should().Be("31926819");
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ParseIban_FormatsCorrectly()
        {
            var result = _parser.ParseIban("GB29NWBK60161331926819") as UnitedKingdomIbanDetails;

            result.Should().NotBeNull();
            result!.FormattedSortCode.Should().Be("60-16-13");
        }

        [Theory]
        [InlineData("GB29 NWBK 6016 1331 9268 19")] // With spaces
        [InlineData("gb29nwbk60161331926819")] // Lowercase
        public void ParseIban_WithFormattedInput_NormalizesAndParses(string iban)
        {
            var result = _parser.ParseIban(iban) as UnitedKingdomIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("GB29NWBK60161331926819");
        }

        [Theory]
        [InlineData("GB00NWBK60161331926819")] // Invalid
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
            => _parser.ParseIban(iban).Should().BeNull();

        [Fact]
        public void Create_ReturnsValidParser()
            => UnitedKingdomIbanParser.Create().CountryCode.Should().Be("GB");

        [Fact]
        public void Create_CreatesNewInstanceEachTime()
        {
            var parser1 = UnitedKingdomIbanParser.Create();
            var parser2 = UnitedKingdomIbanParser.Create();
            parser1.Should().NotBeSameAs(parser2);
        }
    }
}
