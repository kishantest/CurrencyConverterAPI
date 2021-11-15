using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Model
{
    public class CurrencyListInfo
    {
        public bool Success { get; set; }

        public List<CurrencyInfo> Currencies { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class CurrencyInfo
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

    }
  
}
