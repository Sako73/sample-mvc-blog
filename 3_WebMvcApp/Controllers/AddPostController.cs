using DataAccess;
using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMvcApp.Models;

namespace WebMvcApp.Controllers
{
    public class AddPostController : Controller
    {
        private IDataAccess _dataAccess;

        public AddPostController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public string TestAction()
        {
            return "Action!";
        }

        // GET: AddPost
        public ActionResult Index()
        {
            ViewBag.Title = "AddPostView";
            return View("AddPostView");
        }

        // POST: AddPost/AddPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(BlogPostModel blogPost)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ApplicationException("Bad values.");
                }
                var newBp = blogPost.AsBlogPost();
                _dataAccess.AddBlogPost(newBp);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

    }
}
