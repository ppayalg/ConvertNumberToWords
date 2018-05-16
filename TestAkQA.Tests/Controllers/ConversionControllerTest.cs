using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAkQA;
using TestAkQA.Controllers;
using Newtonsoft.Json;
using TestAkQA.Models;

namespace TestAkQA.Tests.Controllers
{

    [TestClass]
    public class ConversionControllerTest
    {
        ConversionController controller;

        public ConversionControllerTest()
        {
            controller = new ConversionController();
        }

        private NameCurrencyModel GetModel(string model)
        {
            return JsonConvert.DeserializeObject<NameCurrencyModel>(model);
        }

        #region Name Validation Test Cases
        [TestMethod]
        public void GetWithNoName()
        {
            string name = "";
            // Act
            var result = controller.Get(name, "100");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.RequiredName, model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithSpaceName()
        {
            string name = " ";
            // Act
            var result = controller.Get(name, "100.55");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.RequiredName, model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithNullName()
        {
            // Act
            var result = controller.Get(null, "100");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.RequiredName, model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithInvalidName()
        {
            // Act
            var result = controller.Get("78787", "100");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.InvalidName, model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithInvalidFullName()
        {
            // Act
            var result = controller.Get("Payal", "100");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.InvalidName, model.ErrorMessage);
        }

        #endregion

        #region Currency Validation Test Cases
        [TestMethod]
        public void GetWithInvalidCurrency()
        {
            // Act
            var result = controller.Get("Payal Gupta", "abcd");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.InvalidCurrency, model.ErrorMessage);
        }

        #endregion

        [TestMethod]
        public void GetWithCurrencyHundredCents()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999.1234");
            var model = GetModel(result);
            // Assert      
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS AND ONE TWO THREE FOUR CENTS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyThousandsCents()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999.12345000");
            var model = GetModel(result);
            // Assert      
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS AND ONE TWO THREE FOUR FIVE ZERO ZERO ZERO CENTS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyZero()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "0");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("ZERO DOLLAR", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyStartZero()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "001");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("ONE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyStartZeroCents()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "001.0");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("ONE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyStartZeroXCents()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "001.01");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("ONE DOLLARS AND ZERO ONE CENTS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyStartZeroXXCents()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "999.99");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE HUNDRED NINETY-NINE DOLLARS AND NINETY-NINE CENTS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyOne()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "1");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("ONE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyOnes()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyTens()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "99");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyHundreds()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyThousands()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }


        [TestMethod]
        public void GetWithCurrencyTenThousands()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "99999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }
        [TestMethod]
        public void GetWithCurrencyHundredThousands()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "999999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyMillions()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE MILLION NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyMillionsCents()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999999.99");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE MILLION NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS AND NINETY-NINE CENTS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyTensMillions()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "99999999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINETY-NINE MILLION NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyHundredMillions()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "999999999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE HUNDRED NINETY-NINE MILLION NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyBillions()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999999999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE BILLION NINE HUNDRED NINETY-NINE MILLION NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyTensBillions()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "99999999999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINETY-NINE BILLION NINE HUNDRED NINETY-NINE MILLION NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyHundredBillions()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "999999999999");
            var model = GetModel(result);
            // Assert            
            Assert.AreEqual(name.ToUpper(), model.Name);
            Assert.AreEqual("NINE HUNDRED NINETY-NINE BILLION NINE HUNDRED NINETY-NINE MILLION NINE HUNDRED NINETY-NINE THOUSAND NINE HUNDRED NINETY-NINE DOLLARS", model.Currency);
            Assert.IsNull(model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyThousandBillions()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999999999999");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.ErrorNotSupported, model.ErrorMessage);
        }

        [TestMethod]
        public void GetWithCurrencyThousandBillionsCents()
        {
            var name = "Payal Gupta";
            // Act
            var result = controller.Get(name, "9999999999999.99");
            var model = GetModel(result);
            // Assert            
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Currency);
            Assert.AreEqual(Constants.ErrorNotSupported, model.ErrorMessage);
        }
    }
}

