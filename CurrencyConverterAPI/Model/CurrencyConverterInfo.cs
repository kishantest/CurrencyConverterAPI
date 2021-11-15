using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Model
{
    public class CurrencyConverterInfo
    {
        public string FromCurrencyCode { get; set; }

        public string ToCurrencyCode { get; set; }

        public decimal Amount { get; set; }

    }

    public class CurrencyExchangeResult
    {
        public string FromCurrencyCode { get; set; }

        public string ToCurrencyCode { get; set; }

        public decimal Amount { get; set; }

        public decimal Rate { get; set; }

        public decimal ConvertedAmount { get; set; }

        public bool Success { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }

}
