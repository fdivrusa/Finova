using Finova.Countries.Europe.Estonia.Models;
using Finova.Countries.Europe.Estonia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Estonia.Services;

public class EstoniaIbanParserTests
{
    private readonly EstoniaIbanParser _parser;

    public EstoniaIbanParserTests()
    {
        _parser = EstoniaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "EE201000001020145686";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<EstoniaIbanDetails>();

        var estoniaDetails = (EstoniaIbanDetails)details!;
        estoniaDetails.CountryCode.Should().Be("EE");
        estoniaDetails.Pangakood.Should().Be("10");
        estoniaDetails.Kontonumber.Should().Be("00001020145686");
    }

    [Fact]
    public void ParseIban_WithNullIban_ReturnsNull()
    {
        _parser.ParseIban(null).Should().BeNull();
    }

    [Fact]
    public void ParseIban_WithInvalidIban_ReturnsNull()
    {
        _parser.ParseIban("invalid").Should().BeNull();
    }
}

