using System;
using System.Web.Mvc;
using WebMvcApp.Controllers;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModels.Models;
using System.Collections.Specialized;

namespace WebMvcApp.Tests.Controllers
{
    [TestClass]
    public class ViewPostControllerTest
    {
        private StaticDataAccess _dataAccess;

        [TestInitialize]
        public void InitializeTests()
        {
            _dataAccess = new StaticDataAccess();
            _dataAccess.Initialize();
        }

        [TestMethod]
        public void ViewPostIndex()
        {
            ViewPostController controller = new ViewPostController(_dataAccess);
            ViewResult result = controller.Index(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("ViewPostView", result.ViewBag.Title);
            var model = (BlogPost)result.Model;
            Assert.AreEqual("This is the first blog post", model.Title);
        }

        [TestMethod]
        public void ViewPostAddComment()
        {
            ViewPostController controller = new ViewPostController(_dataAccess);
            NameValueCollection formData = new NameValueCollection()
            {
                { "newComment", "The new comment" },
                { "postId", "1" }
            };
            FormCollection data = new FormCollection(formData);

            RedirectToRouteResult result = controller.AddComment(data) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.RouteValues["id"]);
            Assert.AreEqual("ViewPost", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            var blogPost = _dataAccess.GetBlogPost(1);
            Assert.AreEqual(5, blogPost.Comments.Count);
        }
    }
}
