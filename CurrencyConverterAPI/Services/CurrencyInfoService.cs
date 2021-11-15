using CurrencyConverterAPI.Helper;
using CurrencyConverterAPI.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace CurrencyConverterAPI.Services
{
    public class CurrencyInfoService : ICurrencyInfoService
    {
        private readonly IConfiguration _configuration;
        private readonly string access_key;
        public CurrencyInfoService (IConfiguration configuration)
        {
            _configuration = configuration;
            access_key = _configuration["ccsecretapikey"];
        }

        public async Task<CurrencyListInfo> GetCurrencies()
        {
            var json = await (new WebClient()).DownloadStringTaskAsync("http://data.fixer.io/api/symbols?access_key="+ access_key);
               
            //return  JsonConvert.DeserializeObject<CurrencyListInfo>(json);

            var jsonObj = JObject.Parse(json);

            var resultList = new CurrencyListInfo
            {
                Success = Convert.ToBoolean(jsonObj["success"]),
                Currencies = new List<CurrencyInfo>(),
                ErrorCode = string.Empty,
                ErrorMessage = string.Empty
            };

            
            if (Convert.ToBoolean(jsonObj["success"]))
            {
                foreach (var item in jsonObj["symbols"])
                {                   
                    foreach (var prop in item)
                    {
                        var property = prop.Parent as JProperty;

                        if (property != null)
                        {
                            resultList.Currencies.Add(new CurrencyInfo() { CurrencyCode = property.Name, CurrencyName = property.Value.ToString() });
                        }

                    }
                }
            }
            else 
            {
                foreach (var item in jsonObj["error"])
                {
                    foreach (var prop in item)
                    {
                        var property = prop.Parent as JProperty;

                        if (property != null)
                        {
                            //example
                            //{
                            //  "success": false,
                            //  "error": {
                            //    "code": 104,
                            //    "info": "Your monthly API request volume has been reached. Please upgrade your plan."
                            //  }
                            //}
                            if (property.Name == "code")
                            {
                                resultList.ErrorCode = property.Value.ToString();
                            }

                            if (property.Name == "info")
                            {
                                resultList.ErrorMessage = property.Value.ToString();
                            }                            
                        }

                    }
                }
            }

            return resultList;
        }

        public async Task<CurrencyExchangeResult> GetCurrencyConvertDetailAsync(CurrencyConverterInfo currencyConvertData)
        {
            using (var wc = new WebClient())
            {
                CurrencyExchangeResult exchangeResult = new CurrencyExchangeResult();
                exchangeResult.FromCurrencyCode = currencyConvertData.FromCurrencyCode;
                exchangeResult.ToCurrencyCode = currencyConvertData.ToCurrencyCode;
                exchangeResult.Amount = currencyConvertData.Amount;

                string json = await wc.DownloadStringTaskAsync("http://data.fixer.io/api/latest?access_key="+ access_key);

                var jsonObj = JObject.Parse(json);

                if (Convert.ToBoolean(jsonObj["success"]))
                {
                    //EUR to from currency exchange rate
                    decimal fromRate = CurrencyConvertHelper.GetLatestExchangeRateForCurrency(currencyConvertData.FromCurrencyCode, json);
                    //EUR to to currency exchange rate
                    decimal toRate = CurrencyConvertHelper.GetLatestExchangeRateForCurrency(currencyConvertData.ToCurrencyCode, json);

                    //error when no from or to currency match found
                    if(fromRate == 0 || toRate == 0)
                    {
                        exchangeResult.Success = false;
                        exchangeResult.Rate = 0;
                        exchangeResult.ConvertedAmount = 0;
                        exchangeResult.ErrorCode = "Error";
                        exchangeResult.ErrorMessage = "From Currency OR To Currency exchange rate not found";

                        return exchangeResult;
                    }

                    //converted exchange rate
                    decimal convertedExchangeRate = CurrencyConvertHelper.ConvertCurrencyExchangeRate(fromRate, toRate);

                    //get amount with converted exchange rate
                    decimal convertedAmount = currencyConvertData.Amount * convertedExchangeRate;

                    exchangeResult.Success = true;
                    exchangeResult.Rate = convertedExchangeRate;
                    exchangeResult.ConvertedAmount = convertedAmount;
                }
                else
                {
                    exchangeResult.Success = false;
                    foreach (var item in jsonObj["error"])
                    {
                        foreach (var prop in item)
                        {
                            var property = prop.Parent as JProperty;

                            if (property != null)
                            {
                                if (property.Name == "code")
                                {
                                    exchangeResult.ErrorCode = property.Value.ToString();
                                }

                                if (property.Name == "info")
                                {
                                    exchangeResult.ErrorMessage = property.Value.ToString();
                                }
                            }

                        }
                    }
                }
                return exchangeResult;
            }           
        }
    }
}
