using Finova.Core.Models;

namespace Finova.Core.Interfaces
{
    public interface IPaymentCardValidator
    {
        bool IsValidLuhn(string? cardNumber);
        PaymentCardBrand GetBrand(string? cardNumber);
        bool IsValidCvv(string? cvv, PaymentCardBrand brand);
        bool IsValidExpiration(int month, int year);
    }
}