using System.Globalization;
using System.Resources;

namespace Finova.Core.Resources;

public static class ValidationMessages
{
    private static readonly ResourceManager ResourceManager = new("Finova.Core.Resources.ValidationMessages", typeof(ValidationMessages).Assembly);

    private static string GetString(string name) => ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? name;

    public static string InvalidCardNumberFormat => GetString("InvalidCardNumberFormat");
    public static string InvalidCardNumberLuhn => GetString("InvalidCardNumberLuhn");
    public static string InvalidCvvDigits => GetString("InvalidCvvDigits");
    public static string InvalidCvvLength => GetString("InvalidCvvLength");
    public static string InvalidMonth => GetString("InvalidMonth");
    public static string CardExpired => GetString("CardExpired");
    public static string CardYearTooFar => GetString("CardYearTooFar");
    public static string InvalidRoutingNumberLength => GetString("InvalidRoutingNumberLength");
    public static string InvalidRoutingNumberFormat => GetString("InvalidRoutingNumberFormat");
    public static string InvalidRoutingNumberChecksum => GetString("InvalidRoutingNumberChecksum");
    public static string InvalidCpfLength => GetString("InvalidCpfLength");
    public static string InvalidCpfFormat => GetString("InvalidCpfFormat");
    public static string InvalidCpfIdenticalDigits => GetString("InvalidCpfIdenticalDigits");
    public static string InvalidCpfChecksum1 => GetString("InvalidCpfChecksum1");
    public static string InvalidCpfChecksum2 => GetString("InvalidCpfChecksum2");
    public static string InvalidCnpjLength => GetString("InvalidCnpjLength");
    public static string InvalidCnpjFormat => GetString("InvalidCnpjFormat");
    public static string InvalidCnpjIdenticalDigits => GetString("InvalidCnpjIdenticalDigits");
    public static string InvalidCnpjChecksum1 => GetString("InvalidCnpjChecksum1");
    public static string InvalidCnpjChecksum2 => GetString("InvalidCnpjChecksum2");
    public static string InvalidCurpLength => GetString("InvalidCurpLength");
    public static string InvalidCurpFormat => GetString("InvalidCurpFormat");
    public static string InvalidCurpChecksum => GetString("InvalidCurpChecksum");
    public static string InvalidRfcLength => GetString("InvalidRfcLength");
    public static string InvalidRfcFormat => GetString("InvalidRfcFormat");
    public static string InvalidUsccLength => GetString("InvalidUsccLength");
    public static string InvalidUsccCharacter => GetString("InvalidUsccCharacter");
    public static string InvalidUsccChecksum => GetString("InvalidUsccChecksum");
    public static string InvalidNricLength => GetString("InvalidNricLength");
    public static string InvalidNricPrefix => GetString("InvalidNricPrefix");
    public static string InvalidNricDigits => GetString("InvalidNricDigits");
    public static string InvalidNricChecksum => GetString("InvalidNricChecksum");
    public static string InvalidCanadaRoutingNumberFormat => GetString("InvalidCanadaRoutingNumberFormat");
    public static string InvalidBsbFormat => GetString("InvalidBsbFormat");
    public static string InvalidIfscFormat => GetString("InvalidIfscFormat");
    public static string InvalidBbanLength => GetString("InvalidBbanLength");
}
