using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMvcApp.Models;

namespace WebMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private IDataAccess _dataAccess;

        public HomeController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public ActionResult Index()
        {
            var blogPosts = _dataAccess.GetBlogPosts(0, 10);
            var bpModels = blogPosts
                .Select(bp => BlogPostModel.FromBlogPost(bp))
                .OrderByDescending(bp => bp.Created)
                .ToList();
            ViewBag.Title = "Cooper Vision Blog Posts";
            return View(bpModels);
        }

        public string TestAction()
        {
            return "Action!";
        }

    }
}