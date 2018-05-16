using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Http;
using TestAkQA.Models;

namespace TestAkQA.Controllers
{
    public class ConversionController : ApiController
    {       
        #region Private Methods

        private bool IsValidName(string name)
        {
            //check regular expression for first name and last name
            var regeX = new Regex(@"^([a-zA-Z]{2,}\s[a-zA-z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)");
            var isMatch = regeX.Match(name);
            return isMatch.Success;
        }

        private string GetJsonModel(NameCurrencyModel model)
        {
            //can convert in xml or json
            return JsonConvert.SerializeObject(model);
        }

        #endregion

        // GET api/values/
        /// <summary>
        /// Convert currency into words
        /// </summary>
        /// <param name="name"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        [HttpGet]
        public string Get(string name, string currency)
        {
            var model = new NameCurrencyModel();
            if (string.IsNullOrWhiteSpace(name))
            {
                model.ErrorMessage = Constants.RequiredName;
                return GetJsonModel(model);
            }
            if (!IsValidName(name))
            {
                model.ErrorMessage = Constants.InvalidName;
                return GetJsonModel(model);
            }
            string isNegative = "";

            double number;
            if (!double.TryParse(currency, out number))
            {
                model.ErrorMessage = Constants.InvalidCurrency;
                return GetJsonModel(model);
            }
            if (currency.Contains("-"))
            {
                isNegative = "MINUS ";
                currency = currency.Substring(1, currency.Length - 1);
            }
            if (currency == "0")
            {
                model.Currency = "ZERO DOLLAR";
            }
            else
            {
                var wordFormat = ConvertToWords.ConvertNumber(currency);
                if (wordFormat == Constants.ErrorNotSupported || wordFormat == Constants.InvalidCurrency)
                {
                    model.ErrorMessage = wordFormat;
                    return GetJsonModel(model);
                }
                else
                {
                    model.Currency = isNegative + wordFormat;
                }
            }
            model.Name = name.ToUpper().Trim();
            return GetJsonModel(model);
        }
    }
}