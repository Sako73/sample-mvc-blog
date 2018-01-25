using System;
using DataAccess;
using DataModels.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebMvcApp.Tests.DataAccess
{
    [TestClass]
    public class StaticDataAccessTest
    {
        private StaticDataAccess _dataAccess;

        [TestInitialize]
        public void InitializeTests()
        {
            _dataAccess = new StaticDataAccess();
            _dataAccess.Initialize();
        }

        [TestMethod]
        public void StaticDataAccessInit()
        {
            Assert.IsNotNull(_dataAccess);
        }

        [TestMethod]
        public void StaticDataAccessGetBlogPosts()
        {
            // NOTE: The (0, 0) is because the paging is not yet implemented.
            var blogPosts = _dataAccess.GetBlogPosts(0, 0);
            Assert.IsNotNull(blogPosts);
            Assert.AreEqual(2, blogPosts.Count);
        }

        [TestMethod]
        public void StaticDataAccessGetBlogPost()
        {
            var blogPost = _dataAccess.GetBlogPost(1);
            Assert.IsNotNull(blogPost);
            Assert.AreEqual(4, blogPost.Comments.Count);
            Assert.AreEqual("This is the first blog post", blogPost.Title);
            Assert.AreEqual(StaticDataAccess.loremTxt, blogPost.Post);
            Assert.IsTrue(blogPost.Created > DateTime.Now.AddDays(-2.1) && blogPost.Created <= DateTime.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void StaticDataAccessGetBlogPostEx()
        {
            var blogPost = _dataAccess.GetBlogPost(-1);
        }

        [TestMethod]
        public void StaticDataAccessAddBlogPost()
        {
            var blogPost = new BlogPost()
            {
                ID = 3,
                Post = "The post",
                Title = "The title",
                Created = new DateTime(2000, 1, 1)
            };
            var newBp = _dataAccess.AddBlogPost(blogPost);
            Assert.IsNotNull(newBp);
            Assert.AreEqual(blogPost.Comments.Count, newBp.Comments.Count);
            Assert.AreEqual(newBp.Title, blogPost.Title);
            Assert.AreEqual(newBp.Post, blogPost.Post);
            Assert.AreEqual(newBp.Created, blogPost.Created);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void StaticDataAccessAddBlogPostEx()
        {
            var existBp = _dataAccess.GetBlogPost(1);
            var blogPost = new BlogPost()
            {
                ID = 3,
                Post = "The post",
                Title = existBp.Title,
                Created = new DateTime(2000, 1, 1)
            };
            var newBp = _dataAccess.AddBlogPost(blogPost);
        }

        [TestMethod]
        public void StaticDataAccessAddComment()
        {
            var blogComment = new BlogComment()
            {
                PostId = 1,
                Comment = "The comment",
                Created = new DateTime(2000, 1, 1)
            };
            _dataAccess.AddComment(blogComment);
            var blogPost = _dataAccess.GetBlogPost(1);
            Assert.AreEqual(5, blogPost.Comments.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void StaticDataAccessAddCommentEx()
        {
            var blogComment = new BlogComment()
            {
                PostId = -1,
                Comment = "The comment",
                Created = new DateTime(2000, 1, 1)
            };
            _dataAccess.AddComment(blogComment);
        }

    }
}
