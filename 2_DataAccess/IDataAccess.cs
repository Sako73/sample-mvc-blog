using DataModels.Models;
using System.Collections.Generic;

namespace DataAccess
{
    public interface IDataAccess
    {
        IList<BlogPost> GetBlogPosts(int startNum, int numToReturn);
        BlogPost GetBlogPost(int id);
        void AddComment(BlogComment blogComment);
        BlogPost AddBlogPost(BlogPost blogPost);
    }
}