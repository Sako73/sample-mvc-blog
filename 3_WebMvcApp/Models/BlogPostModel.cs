using DataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMvcApp.Models
{
    public class BlogPostModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The title cannot be more than 265 characters")]
        public string Title { get; set; }

        [Required]
        public string Post { get; set; }

        public DateTime Created { get; set; }
        public int ID { get; set; }

        public string GetPostSummary()
        {
            if (Post.Length <= 300)
            {
                return Post;
            }
            var summary = Post.Substring(0, 300) + " ...";
            return summary;
        }

        internal BlogPost AsBlogPost()
        {
            var blogPost = new BlogPost()
            {
                Title = this.Title,
                Post = this.Post,
                Created = this.Created
            };

            return blogPost;
        }

        internal static BlogPostModel FromBlogPost(BlogPost blogPost)
        {
            var bpModel = new BlogPostModel()
            {
                Title = blogPost.Title,
                Post = blogPost.Post,
                Created = blogPost.Created,
                ID = blogPost.ID
            };

            return bpModel;
        }
    }
}