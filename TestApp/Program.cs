using BankingHelper.Core.Internals;
using BankingHelper.Belgium.Services;
using BankingHelper.Core.Models;

// Test Modulo97
Console.WriteLine("=== Modulo97 Tests ===");
Console.WriteLine($"123 mod 97 = {Modulo97Helper.Calculate("123")}");
Console.WriteLine($"1234 mod 97 = {Modulo97Helper.Calculate("1234")}");
Console.WriteLine($"12345 mod 97 = {Modulo97Helper.Calculate("12345")}");
Console.WriteLine($"123456 mod 97 = {Modulo97Helper.Calculate("123456")}");
Console.WriteLine($"1234567890 mod 97 = {Modulo97Helper.Calculate("1234567890")}");

// Test OGM generation
Console.WriteLine("\n=== OGM Generation Tests ===");
var service = new BelgianPaymentService();
Console.WriteLine($"1 => {service.Generate("1", PaymentReferenceFormat.Domestic)}");
Console.WriteLine($"123 => {service.Generate("123", PaymentReferenceFormat.Domestic)}");
Console.WriteLine($"1234567890 => {service.Generate("1234567890", PaymentReferenceFormat.Domestic)}");

// Test ISO RF generation
Console.WriteLine("\n=== ISO RF Generation Tests ===");
var iso1 = IsoReferenceHelper.Generate("1");
Console.WriteLine($"1 => {iso1}");
Console.WriteLine($"Valid? {IsoReferenceValidator.IsValid(iso1)}");

var iso2 = IsoReferenceHelper.Generate("539007547034");
Console.WriteLine($"539007547034 => {iso2}");
Console.WriteLine($"Valid? {IsoReferenceValidator.IsValid(iso2)}");
