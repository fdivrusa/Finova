using Finova.Spain.Models;
using Finova.Spain.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Spain.Services
{
    public class SpainIbanParserTests
    {
        private readonly SpainIbanParser _parser;

        public SpainIbanParserTests()
        {
            _parser = SpainIbanParser.Create();
        }

        [Fact]
        public void CountryCode_ReturnsES()
        {
            _parser.CountryCode.Should().Be("ES");
        }

        [Fact]
        public void ParseIban_WithValidSpanishIban_ReturnsDetails()
        {
            var iban = "ES9121000418450200051332";
            var result = _parser.ParseIban(iban);

            result.Should().NotBeNull();
            result.Should().BeOfType<SpainIbanDetails>();
        }

        [Fact]
        public void ParseIban_ExtractsAllComponents()
        {
            var iban = "ES9121000418450200051332";
            var result = _parser.ParseIban(iban) as SpainIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("ES9121000418450200051332");
            result.CountryCode.Should().Be("ES");
            result.CheckDigits.Should().Be("91");
            result.Entidad.Should().Be("2100");
            result.Oficina.Should().Be("0418");
            result.DC.Should().Be("45");
            result.Cuenta.Should().Be("0200051332");
            result.BankCode.Should().Be("2100"); // Entidad maps to BankCode
            result.BranchCode.Should().Be("0418"); // Oficina maps to BranchCode
            result.NationalCheckKey.Should().Be("45"); // DC maps to NationalCheckKey
            result.AccountNumber.Should().Be("0200051332"); // Cuenta maps to AccountNumber
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("ES91 2100 0418 4502 0005 1332")] // With spaces
        [InlineData("es9121000418450200051332")] // Lowercase
        public void ParseIban_WithFormattedInput_NormalizesAndParses(string iban)
        {
            var result = _parser.ParseIban(iban) as SpainIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("ES9121000418450200051332");
            result.Entidad.Should().Be("2100");
            result.Oficina.Should().Be("0418");
        }

        [Fact]
        public void ParseIban_ExtractsDCCorrectly()
        {
            var iban = "ES9121000418450200051332";
            var result = _parser.ParseIban(iban) as SpainIbanDetails;

            result.Should().NotBeNull();
            result!.DC.Should().Be("45");
            result.DC.Should().HaveLength(2);
            result.NationalCheckKey.Should().Be("45");
        }

        [Theory]
        [InlineData("ES0021000418450200051332")] // Wrong check digits
        [InlineData("IT60X0542811101000000123456")] // Wrong country
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        [InlineData("ES9121000418")] // Too short
        public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
        {
            _parser.ParseIban(iban).Should().BeNull();
        }

        [Fact]
        public void ParseIban_WithDifferentBank_ParsesCorrectly()
        {
            var iban = "ES6621000418401234567891";
            var result = _parser.ParseIban(iban) as SpainIbanDetails;

            result.Should().NotBeNull();
            result!.CheckDigits.Should().Be("66");
            result.Entidad.Should().Be("2100");
            result.Cuenta.Should().Be("1234567891");
        }

        [Fact]
        public void CreateDefault_ReturnsValidParser()
        {
            var parser = SpainIbanParser.Create();
            parser.Should().NotBeNull();
            parser.CountryCode.Should().Be("ES");
        }

        [Fact]
        public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
        {
            var iban = "ES9121000418450200051332";
            var result1 = _parser.ParseIban(iban);
            var result2 = _parser.ParseIban(iban);

            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result1.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void ParseIban_ExtractsBankAndBranchCodesCorrectly()
        {
            var iban = "ES9121000418450200051332";
            var result = _parser.ParseIban(iban) as SpainIbanDetails;

            result.Should().NotBeNull();
            result!.Entidad.Should().Be("2100");
            result.Entidad.Should().HaveLength(4);
            result.Oficina.Should().Be("0418");
            result.Oficina.Should().HaveLength(4);
        }

        [Fact]
        public void ParseIban_ExtractsAccountNumberCorrectly()
        {
            var iban = "ES9121000418450200051332";
            var result = _parser.ParseIban(iban) as SpainIbanDetails;

            result.Should().NotBeNull();
            result!.Cuenta.Should().Be("0200051332");
            result.Cuenta.Should().HaveLength(10);
            result.AccountNumber.Should().Be("0200051332");
        }

        [Fact]
        public void ParseIban_VerifiesAllComponentsAreNumeric()
        {
            var iban = "ES9121000418450200051332";
            var result = _parser.ParseIban(iban) as SpainIbanDetails;

            result.Should().NotBeNull();
            result!.Entidad.All(char.IsDigit).Should().BeTrue();
            result.Oficina.All(char.IsDigit).Should().BeTrue();
            result.DC.All(char.IsDigit).Should().BeTrue();
            result.Cuenta.All(char.IsDigit).Should().BeTrue();
        }
    }
}
