using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periodicals.Controllers;

namespace UnitTests.Tests
{
    [TestClass]
    public class ErrorControllerTest
    {
        [TestMethod]
        public void IndexViewResultRedirectCorrect()
        {
            InvalidOperationException exception = new InvalidOperationException();

            ErrorController controller = new ErrorController();

            ViewResult result = controller.Index(exception.GetType().Name) as ViewResult;

            Assert.AreEqual("NotFound", result.ViewName);
        }
    }
}
