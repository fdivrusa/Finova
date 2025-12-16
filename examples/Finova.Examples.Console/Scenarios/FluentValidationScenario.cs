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
        RunBicIbanConsistencyExample();
    }

    private static void RunExtensionMethodsOverview()
    {
        ConsoleHelper.WriteSubHeader("12", "FluentValidation Extension Methods (Finova.Extensions.FluentValidation)");

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
        ConsoleHelper.WriteSubHeader("13", "FluentValidation - SEPA Payment Example");

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
        ConsoleHelper.WriteSubHeader("14", "FluentValidation - Card Payment");

        var validCard = new CardPaymentRequest
        {
            CardNumber = "4532015112830366",
            CardholderName = "John Doe",
            ExpiryMonth = "12",
            ExpiryYear = "28",
            Cvv = "123",
            Amount = 99.99m
        };

        var invalidCard = new CardPaymentRequest
        {
            CardNumber = "1234567890123456",
            CardholderName = "",
            ExpiryMonth = "13",
            ExpiryYear = "20",
            Cvv = "12",
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

    private static void RunBicIbanConsistencyExample()
    {
        ConsoleHelper.WriteSubHeader("15", "FluentValidation - BIC/IBAN Country Consistency");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Testing BIC country matches IBAN country:");
        Console.ResetColor();

        var consistentTransfer = new InternationalTransfer
        {
            SenderIban = "BE68539007547034",
            SenderBic = "KREDBEBB",      // Belgian BIC matches Belgian IBAN
            RecipientIban = "DE89370400440532013000",
            RecipientBic = "COBADEFF"    // German BIC matches German IBAN
        };

        var inconsistentTransfer = new InternationalTransfer
        {
            SenderIban = "BE68539007547034",
            SenderBic = "COBADEFF",      // German BIC doesn't match Belgian IBAN!
            RecipientIban = "DE89370400440532013000",
            RecipientBic = "KREDBEBB"    // Belgian BIC doesn't match German IBAN!
        };

        var transferValidator = new InternationalTransferValidator();

        var consistentResult = transferValidator.Validate(consistentTransfer);
        var inconsistentResult = transferValidator.Validate(inconsistentTransfer);

        Console.WriteLine();
        ConsoleHelper.WriteInfo("Consistent (BE→DE)", $"IsValid = {consistentResult.IsValid}");
        if (consistentResult.IsValid)
        {
            ConsoleHelper.WriteSuccess("BIC countries match IBAN countries!");
        }

        Console.WriteLine();
        ConsoleHelper.WriteInfo("Inconsistent", $"IsValid = {inconsistentResult.IsValid}");
        if (!inconsistentResult.IsValid)
        {
            foreach (var error in inconsistentResult.Errors)
            {
                ConsoleHelper.WriteError($"{error.PropertyName}: {error.ErrorMessage}");
            }
        }

        Console.WriteLine();
    }
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
    public string ExpiryMonth { get; set; } = string.Empty;
    public string ExpiryYear { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public decimal Amount { get; set; }
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

        RuleFor(x => x.ExpiryMonth)
            .NotEmpty().WithMessage("Expiry month is required")
            .Matches(@"^(0[1-9]|1[0-2])$").WithMessage("Invalid expiry month");

        RuleFor(x => x.ExpiryYear)
            .NotEmpty().WithMessage("Expiry year is required")
            .Matches(@"^\d{2}$").WithMessage("Invalid expiry year format");

        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("CVV is required")
            .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive");
    }
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
