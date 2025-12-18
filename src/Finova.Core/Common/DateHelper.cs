using System;

namespace Finova.Core.Common;

/// <summary>
/// Helper methods for date validation.
/// </summary>
public static class DateHelper
{
    /// <summary>
    /// Checks if the given year, month, and day form a valid date.
    /// </summary>
    /// <param name="year">The year (4 digits).</param>
    /// <param name="month">The month (1-12).</param>
    /// <param name="day">The day (1-31).</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool IsValidDate(int year, int month, int day)
    {
        if (year < 1 || year > 9999) return false;
        if (month < 1 || month > 12) return false;
        if (day < 1 || day > DateTime.DaysInMonth(year, month)) return false;
        return true;
    }
}
