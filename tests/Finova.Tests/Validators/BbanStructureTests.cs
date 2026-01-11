using Finova.Core.Identifiers;
using Finova.Services;
using Finova.Countries.Europe.Azerbaijan.Validators;
using Finova.Countries.Europe.Belarus.Validators;
using Xunit;
using Xunit.Abstractions;
using System.Reflection;

namespace Finova.Tests.Validators;

public class BbanStructureTests
{
    private readonly IBbanService _bbanService;
    private readonly ITestOutputHelper _output;

    public BbanStructureTests(ITestOutputHelper output)
    {
        _output = output;

        // Register all IBbanValidator implementations from the Finova assembly
        var validators = typeof(BelarusBbanValidator).Assembly.GetTypes()
            .Where(t => typeof(IBbanValidator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IBbanValidator>()
            .ToList();

        _bbanService = new BbanService(validators);
    }

    [Fact]
    public void Belarus_ParseDetails_ShouldReturnCorrectStructure()
    {
        // BY BBAN: NBRB 3600 9000 0000 2Z00 AB00
        var bban = "NBRB3600900000002Z00AB00";
        var validator = new BelarusBbanValidator();
        var details = validator.ParseDetails(bban);

        Assert.NotNull(details);
        Assert.Equal("BY", details.CountryCode);
        Assert.Equal("NBRB", details.BankCode);
        Assert.Equal("3600", details.BranchCode); // Balance Account
        Assert.Equal("900000002Z00AB00", details.AccountNumber);
    }

    [Fact]
    public void Azerbaijan_ParseDetails_ShouldReturnCorrectStructure()
    {
        // AZ BBAN: NABZ 0000 0000 1370 1000 1944
        var bban = "NABZ00000000137010001944";
        var validator = new AzerbaijanBbanValidator();
        var details = validator.ParseDetails(bban);

        Assert.NotNull(details);
        Assert.Equal("AZ", details.CountryCode);
        Assert.Equal("NABZ", details.BankCode);
        Assert.Equal("00000000137010001944", details.AccountNumber);
        Assert.Null(details.BranchCode); // Not used in AZ
    }

    [Fact]
    public void CrossValidation_Belarus_Azerbaijan_Overlap()
    {
        // Documenting the structural overlap:
        // A valid Azerbaijan BBAN (4 letters + 20 chars) CAN be a valid Belarus BBAN
        // IF the chars at 4-8 are digits (matching Belarus 'Balance Account' requirement).

        // Case 1: AZ BBAN that matches BY structure
        var azBbanWithDigits = "NABZ00000000137010001944"; // 0000 are digits
        var byResult = _bbanService.Validate("BY", azBbanWithDigits);
        
        // This is structurally valid for BY, so we expect True.
        // We cannot distinguish without bank code database.
        Assert.True(byResult.IsValid, "AZ BBAN with digits at 4-8 matches BY structure");

        // Case 2: AZ BBAN that does NOT match BY structure
        // AZ allows letters in account number.
        // If we have letters at pos 4-8, it should fail BY validation.
        var azBbanWithLetters = "NABZABCD0000137010001944"; // ABCD at 4-8
        var azResult = _bbanService.Validate("AZ", azBbanWithLetters);
        Assert.True(azResult.IsValid, "Should be valid AZ BBAN");

        var byResult2 = _bbanService.Validate("BY", azBbanWithLetters);
        Assert.False(byResult2.IsValid, "Should fail BY validation because 4-8 are not digits");
    }

    [Fact]
    public void CrossValidation_Azerbaijan_Belarus_Overlap()
    {
        // Documenting structural overlap:
        // A valid Belarus BBAN (4 alphanum + 4 digits + 16 alphanum)
        // IS ALWAYS a valid Azerbaijan BBAN (4 letters + 20 alphanum)
        // PROVIDED the Bank Code (0-4) contains only letters.

        // Case 1: BY BBAN with Letter Bank Code
        var byBban = "NBRB3600900000002Z00AB00";
        
        var azResult = _bbanService.Validate("AZ", byBban);
        Assert.True(azResult.IsValid, "BY BBAN (with letter bank code) matches AZ structure");

        // Case 2: BY BBAN with Digit in Bank Code (if valid in BY)
        // BY allows alphanumeric Bank Code. AZ allows only Letters.
        var byBbanWithDigit = "12343600900000002Z00AB00"; // '1234' Bank Code
        
        // Assume '1234' is valid bank code for BY (structure wise)
        var byResult = _bbanService.Validate("BY", byBbanWithDigit);
        Assert.True(byResult.IsValid, "Valid BY structure");

        var azResult2 = _bbanService.Validate("AZ", byBbanWithDigit);
        Assert.False(azResult2.IsValid, "Should fail AZ validation because Bank Code contains digits");
    }
}