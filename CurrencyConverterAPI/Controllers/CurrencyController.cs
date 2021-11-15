using CurrencyConverterAPI.Model;
using CurrencyConverterAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyInfoService _currencyInfoService;

        public CurrencyController(ICurrencyInfoService currencyInfoService)
        {
            _currencyInfoService = currencyInfoService;
        }

        // GET api/GetCurrencies  
        [HttpGet("GetCurrencies")]
        public async Task<CurrencyListInfo> GetCurrencies()
        {           
            return await _currencyInfoService.GetCurrencies();  
        }

        // POST api/GetCurrencyExchangeDetail  
        [HttpPost("GetCurrencyExchangeDetail")]
        public async Task<ActionResult<CurrencyExchangeResult>> GetCurrencyExchangeDetail([FromBody] CurrencyConverterInfo converterInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest();
        
            var result = await _currencyInfoService.GetCurrencyConvertDetailAsync(converterInfo);           

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
