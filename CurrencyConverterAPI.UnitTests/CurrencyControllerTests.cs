using CurrencyConverterAPI.Controllers;
using CurrencyConverterAPI.Model;
using CurrencyConverterAPI.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverterAPI.CurrencyControllerTests
{
    public class CurrencyControllerTests
    {
        private readonly CurrencyController _curuat;
        private readonly Mock<ICurrencyInfoService> _currencyInfoServiceMock = new Mock<ICurrencyInfoService>();

        public CurrencyControllerTests()
        {
            _curuat = new CurrencyController(_currencyInfoServiceMock.Object);
        }
        
        [Fact]
        public async Task GetCurrencyExchangeDetail_ShouldError404_When_InvalidModelData()
        {
            //Arrange
            CurrencyConverterInfo cci = new CurrencyConverterInfo();
           
            //Act
            // force validation error 
            _curuat.ModelState.AddModelError("NoInput", "Input is Required");
            var exchangeResult = await _curuat.GetCurrencyExchangeDetail(cci);

            //Assert
            Assert.Equal(400,((Microsoft.AspNetCore.Mvc.StatusCodeResult)exchangeResult.Result).StatusCode);
        }

        [Fact]
        public async Task GetCurrencyExchangeDetail_ShouldReturnError_When_FromCurrency_MatchNotFound()
        {
            //Arrange
            CurrencyConverterInfo cci = new CurrencyConverterInfo();
            cci.FromCurrencyCode = "WRONGCODE";
            cci.ToCurrencyCode = "USD";
            cci.Amount = 100;

            var mckExchangeResult = new CurrencyExchangeResult();
            mckExchangeResult.Amount = 0;
            mckExchangeResult.ConvertedAmount = 0;
            mckExchangeResult.ErrorCode = "Error";
            mckExchangeResult.ErrorMessage = "From Currency OR To Currency exchange rate not found";

            _currencyInfoServiceMock.Setup(x => x.GetCurrencyConvertDetailAsync(cci)).ReturnsAsync(mckExchangeResult);

            //Act
            var exchangeResult = await _curuat.GetCurrencyExchangeDetail(cci);


            //Assert
            Assert.Equal(mckExchangeResult.ErrorMessage, ((CurrencyExchangeResult)((Microsoft.AspNetCore.Mvc.ObjectResult)exchangeResult.Result).Value).ErrorMessage);
        }
    }
}
