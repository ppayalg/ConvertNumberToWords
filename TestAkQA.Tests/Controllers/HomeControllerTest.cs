using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAkQA.Controllers;

namespace TestAkQA.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController controller;
        [TestMethod]
        public void Index()
        {
            // Arrange
            controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test AKQA", result.ViewBag.Title);
        }
    }
}
