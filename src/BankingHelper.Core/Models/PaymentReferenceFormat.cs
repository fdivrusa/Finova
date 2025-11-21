using System;
using System.Collections.Generic;
using System.Text;

namespace BankingHelper.Core.Models
{
    public enum PaymentReferenceFormat
    {
        /// <summary>
        /// Specific format used in Belgium (ex: 123/4567/89012).
        /// </summary>
        Domestic,

        /// <summary>
        /// ISO standard format (ex: RF18 1234 5678 9012).
        /// </summary>
        IsoRf
    }
}
