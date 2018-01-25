using System;
using System.Web.Mvc;
using WebMvcApp.Controllers;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModels.Models;
using WebMvcApp.Models;

namespace WebMvcApp.Tests.Controllers
{
    [TestClass]
    public class AddPostControllerTest
    {
        private StaticDataAccess _dataAccess;

        [TestInitialize]
        public void InitializeTests()
        {
            _dataAccess = new StaticDataAccess();
            _dataAccess.Initialize();
        }

        [TestMethod]
        public void AddPostIndex()
        {
            AddPostController controller = new AddPostController(_dataAccess);
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("AddPostView", result.ViewBag.Title);
        }

        [TestMethod]
        public void AddPostAddPost()
        {
            AddPostController controller = new AddPostController(_dataAccess);
            var blogPost = new BlogPostModel()
            {
                Post = "The post",
                Title = "The title"
            };
            RedirectToRouteResult result = controller.AddPost(blogPost) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            var bp = _dataAccess.GetBlogPost(3);
            Assert.AreEqual(0, bp.Comments.Count);
            Assert.AreEqual("The post", bp.Post);
            Assert.AreEqual("The title", bp.Title);
        }
    }
}
