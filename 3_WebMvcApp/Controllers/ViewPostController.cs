using DataAccess;
using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvcApp.Controllers
{
    public class ViewPostController : Controller
    {
        private IDataAccess _dataAccess;

        public ViewPostController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // GET: ViewPost
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return View("Error", new ApplicationException("id cannot be null"));
            }

            BlogPost blogPost;
            try
            {
                blogPost = _dataAccess.GetBlogPost((int)id);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }

            ViewBag.Title = "ViewPostView";

            return View("ViewPostView", blogPost);
        }

        // POST: ViewPost/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(FormCollection collection)
        {
            // NOTE: XSS is automatically taken care of by MVC.
            try
            {
                var comment = collection["newComment"];
                var id = int.Parse(collection["postId"]);

                // TODO: Add model validation.
                if (!string.IsNullOrWhiteSpace(comment))
                {
                    var blogComment = new BlogComment()
                    {
                        PostId = id,
                        Comment = comment,
                        Created = DateTime.Now
                    };
                    _dataAccess.AddComment(blogComment);
                }

                return RedirectToAction("Index", "ViewPost", new { id = id });
            }
            catch (Exception ex)
            {
                // TODO: Log error
                return View("Error", ex);
            }
        }


    }
}
