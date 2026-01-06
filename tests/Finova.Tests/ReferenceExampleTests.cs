using Finova.Core.Identifiers;
using Finova.Services;
using Xunit;
using Xunit.Abstractions;

namespace Finova.Tests;

public class ExampleVerificationTests
{
    private readonly ITestOutputHelper _output;

    public ExampleVerificationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void VerifyAndFix_Mongolia_Iban()
    {
        // Calculate valid MN IBAN
        // Format: MNkk bbbb aaaa aaaa aaaa (20 chars)
        // Bank: 0001, Account: 000000000000
        string country = "MN";
        string bban = "0001000000000000"; // 16 digits
        string prefix = bban + country + "00";

        // Convert letters to numbers (A=10...Z=35)
        string numeric = "";
        foreach (char c in prefix)
        {
            if (char.IsLetter(c))
            {
                numeric += (c - 'A' + 10).ToString();
            }
            else
            {
                numeric += c;
            }
        }

        // Mod 97
        System.Numerics.BigInteger bigNum = System.Numerics.BigInteger.Parse(numeric);
        int mod = (int)(bigNum % 97);
        int check = 98 - mod;

        string iban = $"{country}{check:D2}{bban}";
        _output.WriteLine($"Valid MN IBAN: {iban}");

        Assert.True(GlobalIbanValidator.ValidateIban(iban).IsValid, "Generated MN IBAN should be valid");
    }

    [Fact]
    public void Verify_All_Bban_Examples()
    {
        // I'll paste the list from BbanValidator.razor here manually for testing
        var examples = new Dictionary<string, string>
        {
            {"DZ", "00100000000000000000"}, // Algeria
            {"AO", "060000000000000000000"}, // Angola
            {"BJ", "B01234567890123456789012"}, // Benin
            {"BF", "B01234567890123456789012"}, // Burkina Faso
            {"BI", "12345678901234567890123"}, // Burundi (Updated to 23)
            {"CM", "37123456789012345678901"}, // Cameroon
            {"CV", "351234567890123456789"}, // Cape Verde
            {"CF", "05123456789012345678901"}, // CAR
            {"TD", "13123456789012345678901"}, // Chad
            {"KM", "39123456789012345678901"}, // Comoros
            {"CG", "13123456789012345678901"}, // Congo
            {"CI", "B01234567890123456789012"}, // Cote d'Ivoire
            {"DJ", "14123456789012345678901"}, // Djibouti
            {"EG", "3800190005000000002631800"}, // Egypt
            {"GQ", "11123456789012345678901"}, // Eq Guinea
            {"GA", "13123456789012345678901"}, // Gabon
            {"GW", "B01234567890123456789012"}, // Guinea Bissau
            {"HN", "BANK12345678901234567890"}, // Honduras
            {"IQ", "BANK123123456789012"}, // Iraq
            {"KW", "CBKU0000000000001234560101"}, // Kuwait
            {"LB", "099900000001001901229114"}, // Lebanon
            {"LY", "121234567890123456789"}, // Libya
            {"MG", "12123456789012345678901"}, // Madagascar
            {"ML", "B01234567890123456789012"}, // Mali
            {"MR", "13000200010100001234567"}, // Mauritania
            {"MZ", "121234567890123456789"}, // Mozambique
            {"NI", "BANK123456789012345678901234"}, // Nicaragua
            {"NE", "B01234567890123456789012"}, // Niger
            {"PS", "PALS000000000400123456702"}, // Palestine
            {"QA", "DOHB000012345678901234567"}, // Qatar
            {"LC", "BANK123456789012345678901234"}, // Saint Lucia
            {"ST", "121234567890123456789"}, // Sao Tome
            {"SA", "0380000000608010167519"}, // Saudi Arabia
            {"SN", "B01234567890123456789012"}, // Senegal
            {"SC", "BANK12345678901234567890CUR"}, // Seychelles
            {"SO", "1212345678901234567"}, // Somalia
            {"SD", "12123456789012"}, // Sudan
            {"TG", "B01234567890123456789012"}, // Togo
            {"TN", "59100060351835984788"}, // Tunisia
            {"AE", "070331234567890123456"}, // UAE
            {"YE", "BANK1234567890123456789012"}, // Yemen
            {"MA", "007810000252600000032955"}, // Morocco
            {"RU", "04452560040702810400000000012"}, // Russia
            {"MN", "0001000000000000"}, // Mongolia
            {"OM", "1234567890123456789"}, // Oman (Updated to 19)
            {"BB", "CITB00000000000012345678"}, // Barbados (Updated to 24)
            {"FK", "SC123456789012"}, // Falkland
        };

        // We need to instantiate BbanService with all validators.
        // This is complex in a unit test without DI setup.
        // Instead, I'll instantiate specific validators or rely on the fact that I just want to check the strings structure/logic.
        // Actually, I can use the specific country validator classes directly if they are public.

        // I will check a few critical ones manually here using reflection or just direct instantiation if possible.
        // Since I can't easily register all 100+ validators here without copying the DI logic, 
        // I will focus on the reported ones and the new ones.

        CheckBban("MN", examples["MN"], new Finova.Countries.Asia.Mongolia.Validators.MongoliaBbanValidator());
        CheckBban("MA", examples["MA"], new Finova.Countries.Africa.Morocco.Validators.MoroccoBbanValidator());
        CheckBban("RU", examples["RU"], new Finova.Countries.Europe.Russia.Validators.RussiaBbanValidator());
        CheckBban("OM", examples["OM"], new Finova.Countries.MiddleEast.Oman.Validators.OmanBbanValidator());
        CheckBban("BB", examples["BB"], new Finova.Countries.NorthAmerica.Barbados.Validators.BarbadosBbanValidator());
        CheckBban("FK", examples["FK"], new Finova.Countries.SouthAmerica.FalklandIslands.Validators.FalklandIslandsBbanValidator());

        // Check Burundi as well since I changed it
        CheckBban("BI", examples["BI"], new Finova.Countries.Africa.Burundi.Validators.BurundiBbanValidator());
    }

    private void CheckBban(string country, string bban, IBbanValidator validator)
    {
        var result = validator.Validate(bban);
        if (!result.IsValid)
        {
            _output.WriteLine($"Invalid BBAN for {country}: {bban}. Errors: {string.Join(", ", result.Errors.Select(e => e.Message))}");
        }
        Assert.True(result.IsValid, $"BBAN for {country} should be valid");
    }
}
