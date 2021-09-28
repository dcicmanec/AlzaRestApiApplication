using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlzaRestApiApplication.Controllers;

namespace AlzaRestApiApplication.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index(1, true) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Detail()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Detail(1, true) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Edit(1, true) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
