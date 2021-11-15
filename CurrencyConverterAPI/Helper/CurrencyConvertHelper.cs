using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Helper
{
    public static class CurrencyConvertHelper
    {
        // Converts one currency to another via the Euro exchange rate 
        public static decimal ConvertCurrencyExchangeRate(decimal firstCurrency, decimal secondCurrency)
        {
            decimal convertedCurrency = (1 / firstCurrency) * secondCurrency;
            return convertedCurrency;
        }

        // Parses a string of latest exchange rates to EUR
        public static decimal GetLatestExchangeRateForCurrency(string currencyCode, string jsonString)
        {
            try
            {
                decimal amount = 0.0M;
               
                //Removes the headers
                string[] splitInformation = jsonString.Split('{', '}');
                //Data can split into Name : Rate
                string[] rows = splitInformation[2].Split(',');
                foreach (var row in rows)
                {
                    //Gets rid of spaces and new line characters
                    string line = row.Trim();
                    //Gets the currency code name
                    string name = line.Substring(1, 3);
                    if (name == currencyCode)
                    {
                        //extracts the rate
                        amount = Convert.ToDecimal(line.Substring(6));
                        return amount;
                    }
                }
            }
            catch
            {
                return 0;
            }
            return 0;
        }
    }
}
