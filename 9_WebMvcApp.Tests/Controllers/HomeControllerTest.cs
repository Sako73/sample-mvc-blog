using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMvcApp;
using WebMvcApp.Controllers;
using DataAccess;
using DataModels.Models;
using WebMvcApp.Models;

namespace WebMvcApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private StaticDataAccess _dataAccess;

        [TestInitialize]
        public void InitializeTests()
        {
            _dataAccess = new StaticDataAccess();
            _dataAccess.Initialize();
        }

        [TestMethod]
        public void HomeIndex()
        {
            HomeController controller = new HomeController(_dataAccess);
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Cooper Vision Blog Posts", result.ViewBag.Title);
            Assert.IsNotNull(result.Model);
            var model = (IList<BlogPostModel>)result.Model;
            Assert.AreEqual(2, model.Count);
        }

    }
}
