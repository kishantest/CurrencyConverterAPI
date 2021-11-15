using CurrencyConverterAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Services
{
    public interface ICurrencyInfoService
    {
        Task<CurrencyListInfo> GetCurrencies();

        Task<CurrencyExchangeResult> GetCurrencyConvertDetailAsync(CurrencyConverterInfo currencyConvertData);
    }
}
