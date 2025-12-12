using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Spain.Validators;

public partial class SpainVatValidator : IVatValidator
{
    // Spain VAT (NIF) format: 9 characters.
    // 1. 8 digits + 1 letter (DNI)
    // 2. 1 letter + 7 digits + 1 letter (NIE)
    // 3. 1 letter + 8 digits (CIF)
    // Simplified regex: 9 alphanumeric characters.
    [GeneratedRegex(@"^[A-Z0-9]{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "ES";

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Spain VAT format.");
        }

        // Checksum Validation
        char firstChar = cleaned[0];
        char lastChar = cleaned[8];
        string digits = cleaned.Substring(1, 7);

        if (char.IsDigit(firstChar))
        {
            // DNI (National entities)
            // 8 digits + 1 letter
            string numberPart = cleaned.Substring(0, 8);
            if (!long.TryParse(numberPart, out long number))
            {
                // Should be digits if first char is digit
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid DNI format.");
            }

            string controlChars = "TRWAGMYFPDXBNJZSQVHLCKE";
            char expectedChar = controlChars[(int)(number % 23)];

            if (lastChar != expectedChar)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid DNI checksum.");
            }
        }
        else if (firstChar == 'X' || firstChar == 'Y' || firstChar == 'Z')
        {
            // NIE (Foreigners)
            // Replace X->0, Y->1, Z->2
            string prefix = firstChar == 'X' ? "0" : firstChar == 'Y' ? "1" : "2";
            string numberPart = prefix + cleaned.Substring(1, 7);

            if (!long.TryParse(numberPart, out long number))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid NIE format.");
            }

            string controlChars = "TRWAGMYFPDXBNJZSQVHLCKE";
            char expectedChar = controlChars[(int)(number % 23)];

            if (lastChar != expectedChar)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid NIE checksum.");
            }
        }
        else
        {
            // CIF (Juridical entities)
            // Letter + 7 digits + Control (Letter or Digit)
            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                int digit = digits[i] - '0';
                if (i % 2 == 0) // Odd positions (1st, 3rd... in digits string)
                {
                    int doubled = digit * 2;
                    sum += (doubled / 10) + (doubled % 10);
                }
                else
                {
                    sum += digit;
                }
            }

            int controlDigit = (10 - (sum % 10)) % 10;
            char[] controlLetters = { 'J', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
            char expectedLetter = controlLetters[controlDigit];

            bool validDigit = char.IsDigit(lastChar) && (lastChar - '0' == controlDigit);
            bool validLetter = lastChar == expectedLetter;

            if (!validDigit && !validLetter)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid CIF checksum.");
            }
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = VatPrefix,
            VatNumber = cleaned,
            IsValid = true
        };
    }
}
