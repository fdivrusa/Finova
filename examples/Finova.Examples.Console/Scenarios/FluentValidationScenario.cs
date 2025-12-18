using Finova.Examples.ConsoleApp.Helpers;
using Finova.Extensions.FluentValidation;
using FluentValidation;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class FluentValidationScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 3: FLUENTVALIDATION INTEGRATION");
        RunExtensionMethodsOverview();
        RunSepaPaymentExample();
        RunCardPaymentExample();
        RunGlobalTaxIdExample();
        RunBankValidationExample();
        RunNationalIdExample();
        // RunBicIbanConsistencyExample();
    }

    private static void RunExtensionMethodsOverview()
    {
        ConsoleHelper.WriteSubHeader("15", "FluentValidation Extension Methods (Finova.Extensions.FluentValidation)");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("      Available extension methods from FinovaValidators:");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("      ┌─────────────────────────────────────────────────────────────────────┐");
        Console.WriteLine("      │  Extension Method              │  Purpose                           │");
        Console.WriteLine("      ├─────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("      │  .MustBeValidIban()            │  Validates IBAN (any EU country)   │");
        Console.WriteLine("      │  .MustBeValidBic()             │  Validates BIC/SWIFT code          │");
        Console.WriteLine("      │  .MustBeValidPaymentCard()     │  Validates card (Luhn algorithm)   │");
        Console.WriteLine("      │  .MustMatchIbanCountry(iban)   │  BIC country must match IBAN       │");
        Console.WriteLine("      └─────────────────────────────────────────────────────────────────────┘");
        Console.ResetColor();
        Console.WriteLine();

        // Demo: MustBeValidIban()
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("      ► MustBeValidIban() - Validates IBAN format and checksum");
        Console.ResetColor();
        ConsoleHelper.WriteCode("RuleFor(x => x).MustBeValidIban()");

        var ibanDemoValidator = new InlineValidator<string>();
        ibanDemoValidator.RuleFor(x => x).MustBeValidIban().WithMessage("Invalid IBAN");

        string[] ibanTestCases = ["BE68539007547034", "FR763000600001123456789", "INVALID123"];
        foreach (var iban in ibanTestCases)
        {
            var result = ibanDemoValidator.Validate(iban);
            ConsoleHelper.WriteSimpleResult(iban, result.IsValid);
        }
        Console.WriteLine();

        // Demo: MustBeValidBic()
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("      ► MustBeValidBic() - Validates BIC/SWIFT code format");
        Console.ResetColor();
        ConsoleHelper.WriteCode("RuleFor(x => x).MustBeValidBic()");

        var bicDemoValidator = new InlineValidator<string>();
        bicDemoValidator.RuleFor(x => x).MustBeValidBic().WithMessage("Invalid BIC");

        string[] bicTestCases = ["KREDBEBB", "COBADEFF", "NWBKGB2L", "INVALID"];
        foreach (var bic in bicTestCases)
        {
            var result = bicDemoValidator.Validate(bic);
            ConsoleHelper.WriteSimpleResult(bic, result.IsValid);
        }
        Console.WriteLine();

        // Demo: MustBeValidPaymentCard()
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("      ► MustBeValidPaymentCard() - Validates card using Luhn algorithm");
        Console.ResetColor();
        ConsoleHelper.WriteCode("RuleFor(x => x).MustBeValidPaymentCard()");

        var cardDemoValidator = new InlineValidator<string>();
        cardDemoValidator.RuleFor(x => x).MustBeValidPaymentCard().WithMessage("Invalid card");

        string[] cardTestCases = ["4111111111111111", "5500000000000004", "1234567890123456"];
        foreach (var card in cardTestCases)
        {
            var result = cardDemoValidator.Validate(card);
            ConsoleHelper.WriteSimpleResult(card, result.IsValid);
        }
        Console.WriteLine();

        // Demo: MustMatchIbanCountry()
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("      ► MustMatchIbanCountry(iban) - Validates BIC country matches IBAN country");
        Console.ResetColor();
        ConsoleHelper.WriteCode("RuleFor(x => x.Bic).MustMatchIbanCountry(x => x.Iban)");

        var bicCountryTests = new[]
        {
            new { Bic = "KREDBEBB", Iban = "BE68539007547034", Desc = "Belgian BIC + Belgian IBAN" },
            new { Bic = "COBADEFF", Iban = "DE89370400440532013000", Desc = "German BIC + German IBAN" },
            new { Bic = "KREDBEBB", Iban = "DE89370400440532013000", Desc = "Belgian BIC + German IBAN (mismatch!)" }
        };

        foreach (var test in bicCountryTests)
        {
            var bicCountryValidator = new InlineValidator<(string Bic, string Iban)>();
            bicCountryValidator.RuleFor(x => x.Bic)
                .MustMatchIbanCountry(x => x.Iban)
                .WithMessage("BIC country must match IBAN country");

            var result = bicCountryValidator.Validate((test.Bic, test.Iban));
            var status = result.IsValid ? "✓" : "✗";
            var color = result.IsValid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.ForegroundColor = color;
            Console.WriteLine($"      {status} {test.Desc}");
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    private static void RunSepaPaymentExample()
    {
        ConsoleHelper.WriteSubHeader("16", "FluentValidation - SEPA Payment Example");
        ConsoleHelper.WriteCode("sepaValidator.Validate(validPayment)");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Using validators in a real-world SEPA payment scenario:");
        Console.ResetColor();

        // Define a sample payment request
        var validPayment = new SepaPaymentRequest
        {
            DebtorIban = "BE68539007547034",
            DebtorBic = "KREDBEBB",
            CreditorIban = "DE89370400440532013000",
            CreditorBic = "COBADEFF",
            Amount = 1500.00m,
            Currency = "EUR"
        };

        var invalidPayment = new SepaPaymentRequest
        {
            DebtorIban = "INVALID_IBAN",
            DebtorBic = "BAD",
            CreditorIban = "XX00000000000000",
            CreditorBic = "TOOLONGBICCODE",
            Amount = -100m,
            Currency = "USD"
        };

        var sepaValidator = new SepaPaymentRequestValidator();

        var validResult = sepaValidator.Validate(validPayment);
        var invalidResult = sepaValidator.Validate(invalidPayment);

        Console.WriteLine();
        ConsoleHelper.WriteInfo("Valid Payment", $"IsValid = {validResult.IsValid}");
        if (validResult.IsValid)
        {
            ConsoleHelper.WriteSuccess("All validation rules passed!");
        }

        Console.WriteLine();
        ConsoleHelper.WriteInfo("Invalid Payment", $"IsValid = {invalidResult.IsValid}");
        if (!invalidResult.IsValid)
        {
            foreach (var error in invalidResult.Errors)
            {
                ConsoleHelper.WriteError($"{error.PropertyName}: {error.ErrorMessage}");
            }
        }

        Console.WriteLine();
    }

    private static void RunCardPaymentExample()
    {
        ConsoleHelper.WriteSubHeader("17", "FluentValidation - Card Payment");
        ConsoleHelper.WriteCode("cardPaymentValidator.Validate(validCard)");

        var validCard = new CardPaymentRequest
        {
            CardNumber = "4532015112830366", // Visa (starts with 4)
            CardholderName = "John Doe",
            ExpiryMonth = 12,
            ExpiryYear = 2028,
            Cvv = "123", // Valid 3 digits for Visa
            Amount = 99.99m
        };

        var invalidCard = new CardPaymentRequest
        {
            CardNumber = "1234567890123456", // Invalid Luhn
            CardholderName = "",
            ExpiryMonth = 13, // Invalid month
            ExpiryYear = 2020, // Expired
            Cvv = "12", // Invalid length
            Amount = 0m
        };

        var cardPaymentValidator = new CardPaymentRequestValidator();

        var validCardResult = cardPaymentValidator.Validate(validCard);
        var invalidCardResult = cardPaymentValidator.Validate(invalidCard);

        ConsoleHelper.WriteInfo("Valid Card", $"IsValid = {validCardResult.IsValid}");
        if (validCardResult.IsValid)
        {
            ConsoleHelper.WriteSuccess("Card payment validation passed!");
        }

        Console.WriteLine();
        ConsoleHelper.WriteInfo("Invalid Card", $"IsValid = {invalidCardResult.IsValid}");
        if (!invalidCardResult.IsValid)
        {
            foreach (var error in invalidCardResult.Errors.Take(5))
            {
                ConsoleHelper.WriteError($"{error.PropertyName}: {error.ErrorMessage}");
            }
        }

        Console.WriteLine();
    }

    private static void RunGlobalTaxIdExample()
    {
        ConsoleHelper.WriteSubHeader("18", "FluentValidation - Global Tax IDs");

        var validator = new GlobalTaxIdValidator();

        var requests = new[]
        {
            new GlobalTaxIdRequest { Country = "US", TaxId = "123456789", Description = "Valid US EIN" },
            new GlobalTaxIdRequest { Country = "US", TaxId = "123", Description = "Invalid US EIN" },
            new GlobalTaxIdRequest { Country = "BR", TaxId = "12345678000195", Description = "Valid Brazil CNPJ" }, // Example CNPJ
            new GlobalTaxIdRequest { Country = "AU", TaxId = "51824753556", Description = "Valid Australia ABN" } // Example ABN
        };

        foreach (var req in requests)
        {
            var result = validator.Validate(req);
            ConsoleHelper.WriteInfo(req.Description, $"IsValid = {result.IsValid}");
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ConsoleHelper.WriteError($"{error.PropertyName}: {error.ErrorMessage}");
                }
            }
        }
        Console.WriteLine();
    }

    private static void RunBankValidationExample()
    {
        ConsoleHelper.WriteSubHeader("19", "Bank Routing & Account Validation");

        Console.WriteLine("      Validating Bank Routing Numbers (US, CA, GB, DE) and Account Numbers (SG, JP, GB).");
        Console.WriteLine();

        var validator = new BankDetailsValidator();

        // US: Only Routing Number is supported for validation in this library
        var validUS = new BankDetails { CountryCode = "US", RoutingNumber = "121000248", AccountNumber = "" }; 
        
        // SG: Only Account Number is supported for validation in this library
        var validSG = new BankDetails { CountryCode = "SG", RoutingNumber = "", AccountNumber = "0011234560" }; 

        // GB: Both are supported
        var validGB = new BankDetails { CountryCode = "GB", RoutingNumber = "20-04-15", AccountNumber = "32456789" }; // Barclays

        // DE: Routing Number (BLZ) is supported
        var validDE = new BankDetails { CountryCode = "DE", RoutingNumber = "10070024", AccountNumber = "" }; // Deutsche Bank Berlin

        // FR: Routing Number (Code Banque) is supported
        var validFR = new BankDetails { CountryCode = "FR", RoutingNumber = "30004", AccountNumber = "" }; // BNP Paribas

        // IT: Routing Number (ABI) is supported
        var validIT = new BankDetails { CountryCode = "IT", RoutingNumber = "01030", AccountNumber = "" }; // Monte dei Paschi di Siena

        // ES: Routing Number (Entidad) is supported
        var validES = new BankDetails { CountryCode = "ES", RoutingNumber = "2100", AccountNumber = "" }; // CaixaBank

        // Invalid Examples
        var invalidUS = new BankDetails { CountryCode = "US", RoutingNumber = "123456789", AccountNumber = "" }; // Invalid Checksum
        var invalidSG = new BankDetails { CountryCode = "SG", RoutingNumber = "", AccountNumber = "123" }; // Too short

        var examples = new[] { validUS, validSG, validGB, validDE, validFR, validIT, validES, invalidUS, invalidSG };

        foreach (var example in examples)
        {
            var result = validator.Validate(example);
            string routingDisplay = string.IsNullOrEmpty(example.RoutingNumber) ? "(none)" : example.RoutingNumber;
            string accountDisplay = string.IsNullOrEmpty(example.AccountNumber) ? "(none)" : example.AccountNumber;

            Console.Write($"      Country: {example.CountryCode}, Routing: {routingDisplay}, Account: {accountDisplay} -> ");
            if (result.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Valid");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid: {result.Errors.FirstOrDefault()?.ErrorMessage}");
            }
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    public class SepaPaymentRequest
    {
        public string DebtorIban { get; set; } = string.Empty;
        public string DebtorBic { get; set; } = string.Empty;
        public string CreditorIban { get; set; } = string.Empty;
        public string CreditorBic { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";
    }

    public class SepaPaymentRequestValidator : AbstractValidator<SepaPaymentRequest>
    {
        public SepaPaymentRequestValidator()
        {
            RuleFor(x => x.DebtorIban)
                .NotEmpty().WithMessage("Debtor IBAN is required")
                .MustBeValidIban().WithMessage("Debtor IBAN is invalid");

            RuleFor(x => x.DebtorBic)
                .NotEmpty().WithMessage("Debtor BIC is required")
                .MustBeValidBic().WithMessage("Debtor BIC is invalid");

            RuleFor(x => x.CreditorIban)
                .NotEmpty().WithMessage("Creditor IBAN is required")
                .MustBeValidIban().WithMessage("Creditor IBAN is invalid");

            RuleFor(x => x.CreditorBic)
                .NotEmpty().WithMessage("Creditor BIC is required")
                .MustBeValidBic().WithMessage("Creditor BIC is invalid");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive");

            RuleFor(x => x.Currency)
                .Equal("EUR").WithMessage("SEPA payments must be in EUR");
        }
    }

    public class CardPaymentRequest
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardholderName { get; set; } = string.Empty;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public Finova.Core.PaymentCard.PaymentCardBrand Brand => Finova.Core.PaymentCard.PaymentCardValidator.GetBrand(CardNumber);
    }

    public class CardPaymentRequestValidator : AbstractValidator<CardPaymentRequest>
    {
        public CardPaymentRequestValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required")
                .MustBeValidPaymentCard().WithMessage("Card number is invalid");

            RuleFor(x => x.CardholderName)
                .NotEmpty().WithMessage("Cardholder name is required");

            // RuleFor(x => x.ExpiryMonth)
            //     .MustBeValidCardExpiration(x => x.ExpiryYear).WithMessage("Card has expired or date is invalid");

            // RuleFor(x => x.Cvv)
            //     .NotEmpty().WithMessage("CVV is required");
            //     //.MustBeValidCardCvv(x => x.Brand).WithMessage("Invalid CVV for this card brand");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive");
        }
    }

    public class GlobalTaxIdRequest
    {
        public string Country { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class GlobalTaxIdValidator : AbstractValidator<GlobalTaxIdRequest>
    {
        public GlobalTaxIdValidator()
        {
            RuleFor(x => x.TaxId)
                .MustBeValidNorthAmericaTaxId(x => x.Country).When(x => x.Country == "US" || x.Country == "CA")
                .MustBeValidSouthAmericaTaxId(x => x.Country).When(x => x.Country == "BR" || x.Country == "MX")
                .MustBeValidAsiaTaxId(x => x.Country).When(x => x.Country == "CN" || x.Country == "IN" || x.Country == "JP" || x.Country == "SG")
                .MustBeValidOceaniaTaxId(x => x.Country).When(x => x.Country == "AU");
        }
    }

    private static void RunNationalIdExample()
    {
        ConsoleHelper.WriteSubHeader("16", "National ID Validation (FluentValidation)");
        ConsoleHelper.WriteCode("RuleFor(x => x).MustBeValidNationalId(countryCode)");

        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x).MustBeValidNationalId("BE").WithMessage("Invalid Belgium NN");

        string[] examples = ["72020290081", "72020290082"];
        foreach (var id in examples)
        {
            var result = validator.Validate(id);
            ConsoleHelper.WriteSimpleResult($"Belgium NN '{id}'", result.IsValid, result.IsValid ? "Valid" : result.Errors.FirstOrDefault()?.ErrorMessage);
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Dynamic Country Code:");
        Console.ResetColor();
        ConsoleHelper.WriteCode("RuleFor(x => x.Id).MustBeValidNationalId(x => x.Country)");

        var dynamicValidator = new InlineValidator<(string Country, string Id)>();
        dynamicValidator.RuleFor(x => x.Id).MustBeValidNationalId(x => x.Country);

        var dynamicExamples = new[]
        {
            ("BE", "72020290081"),
            ("FR", "1 80 01 45 000 000 69"),
            ("IT", "INVALID_CF")
        };

        foreach (var (country, id) in dynamicExamples)
        {
            var result = dynamicValidator.Validate((country, id));
            ConsoleHelper.WriteSimpleResult($"{country} ID '{id}'", result.IsValid);
        }
        Console.WriteLine();
    }

    public class InternationalTransfer
    {
        public string SenderIban { get; set; } = string.Empty;
        public string SenderBic { get; set; } = string.Empty;
        public string RecipientIban { get; set; } = string.Empty;
        public string RecipientBic { get; set; } = string.Empty;
    }

    public class InternationalTransferValidator : AbstractValidator<InternationalTransfer>
    {
        public InternationalTransferValidator()
        {
            RuleFor(x => x.SenderIban)
                .NotEmpty().WithMessage("Sender IBAN is required")
                .MustBeValidIban().WithMessage("Sender IBAN is invalid");

            RuleFor(x => x.SenderBic)
                .NotEmpty().WithMessage("Sender BIC is required")
                .MustBeValidBic().WithMessage("Sender BIC is invalid")
                .MustMatchIbanCountry(x => x.SenderIban).WithMessage("Sender BIC country must match sender IBAN country");

            RuleFor(x => x.RecipientIban)
                .NotEmpty().WithMessage("Recipient IBAN is required")
                .MustBeValidIban().WithMessage("Recipient IBAN is invalid");

            RuleFor(x => x.RecipientBic)
                .NotEmpty().WithMessage("Recipient BIC is required")
                .MustBeValidBic().WithMessage("Recipient BIC is invalid")
                .MustMatchIbanCountry(x => x.RecipientIban).WithMessage("Recipient BIC country must match recipient IBAN country");
        }
    }

    public class BankDetails
    {
        public string CountryCode { get; set; } = string.Empty;
        public string RoutingNumber { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
    }

    public class BankDetailsValidator : AbstractValidator<BankDetails>
    {
        public BankDetailsValidator()
        {
            RuleFor(x => x.RoutingNumber)
                .MustBeValidBankRoutingNumber(x => x.CountryCode)
                .When(x => !string.IsNullOrEmpty(x.RoutingNumber));

            RuleFor(x => x.AccountNumber)
                .MustBeValidBankAccountNumber(x => x.CountryCode)
                .When(x => !string.IsNullOrEmpty(x.AccountNumber));
        }
    }
}
