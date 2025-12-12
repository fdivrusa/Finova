using Finova.Countries.Europe.Italy.Models;
using Finova.Countries.Europe.Italy.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Italy.Services;

public class ItalyIbanParserTests
{
    private readonly ItalyIbanParser _parser;

    public ItalyIbanParserTests()
    {
        _parser = ItalyIbanParser.Create();
    }

    [Fact]
    public void CountryCode_ReturnsIT()
    {
        _parser.CountryCode.Should().Be("IT");
    }

    [Fact]
    public void ParseIban_WithValidItalianIban_ReturnsDetails()
    {
        var iban = "IT60X0542811101000000123456";
        var result = _parser.ParseIban(iban);

        result.Should().NotBeNull();
        result.Should().BeOfType<ItalyIbanDetails>();
    }

    [Fact]
    public void ParseIban_ExtractsAllComponents()
    {
        var iban = "IT60X0542811101000000123456";
        var result = _parser.ParseIban(iban) as ItalyIbanDetails;

        result.Should().NotBeNull();
        result!.Iban.Should().Be("IT60X0542811101000000123456");
        result.CountryCode.Should().Be("IT");
        result.CheckDigits.Should().Be("60");
        result.Cin.Should().Be("X");
        result.Abi.Should().Be("05428");
        result.Cab.Should().Be("11101");
        result.NumeroConto.Should().Be("000000123456");
        result.BankCode.Should().Be("05428"); // ABI maps to BankCode
        result.BranchCode.Should().Be("11101"); // CAB maps to BranchCode
        result.AccountNumber.Should().Be("000000123456"); // NumeroConto maps to AccountNumber
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IT60 X054 2811 1010 0000 0123 456")] // With spaces
    [InlineData("it60x0542811101000000123456")] // Lowercase
    public void ParseIban_WithFormattedInput_NormalizesAndParses(string iban)
    {
        var result = _parser.ParseIban(iban) as ItalyIbanDetails;

        result.Should().NotBeNull();
        result!.Iban.Should().Be("IT60X0542811101000000123456");
        result.Cin.Should().Be("X");
        result.Abi.Should().Be("05428");
    }

    [Theory]
    [InlineData("IT00X0542811101000000123456")] // Wrong check digits
    [InlineData("ES9121000418450200051332")] // Wrong country
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    [InlineData("IT60X054281110")] // Too short
    public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
    {
        _parser.ParseIban(iban).Should().BeNull();
    }

    [Fact]
    public void ParseIban_WithValidCinLetter_ParsesCorrectly()
    {
        // Using the known valid IBAN with CIN 'X'
        var iban = "IT60X0542811101000000123456";
        var result = _parser.ParseIban(iban) as ItalyIbanDetails;

        result.Should().NotBeNull();
        result!.Cin.Should().Be("X");
        result.CheckDigits.Should().Be("60");
    }

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        var parser = ItalyIbanParser.Create();
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("IT");
    }

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        var iban = "IT60X0542811101000000123456";
        var result1 = _parser.ParseIban(iban);
        var result2 = _parser.ParseIban(iban);

        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().BeEquivalentTo(result2);
    }

    [Fact]
    public void ParseIban_ExtractsBankAndBranchCodesCorrectly()
    {
        var iban = "IT60X0542811101000000123456";
        var result = _parser.ParseIban(iban) as ItalyIbanDetails;

        result.Should().NotBeNull();
        result!.Abi.Should().Be("05428");
        result.Abi.Should().HaveLength(5);
        result.Cab.Should().Be("11101");
        result.Cab.Should().HaveLength(5);
    }
}

