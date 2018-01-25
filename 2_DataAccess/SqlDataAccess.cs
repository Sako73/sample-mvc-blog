using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;

namespace DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private ConnectionStringSettings _conStr;

        public SqlDataAccess()
        {
            _conStr = ConfigurationManager.ConnectionStrings["SQLite"];
        }

        private const string _getMaxBlogId = "select max(id) from BlogPost;";
        private const string _insertBlog = "insert into BlogPost (id, title, post, created) values (@id, @title, @post, @created);";
        private const string _getMaxCommentId = "select max(id) from BlogComment;";
        private const string _insertComment = "insert into BlogComment (id, postId, comment, created) values (@id, @postId, @comment, @created);";
        // NOTE: Limit this when there are many posts.
        private const string _getBlogs = "select * from BlogPost;";
        private const string _getBlog = @"
            select * from BlogPost where id = @id; 
            select * from BlogComment where postId = @id order by created;";

        public BlogPost AddBlogPost(BlogPost blogPost)
        {
            using (var connection = new SQLiteConnection(_conStr.ConnectionString))
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.Transaction = trans;

                    cmd.CommandText = _getMaxBlogId;
                    var maxId = cmd.ExecuteScalar();
                    blogPost.ID = maxId is DBNull ? 1 : Convert.ToInt32(maxId) + 1;

                    cmd.CommandText = _insertBlog;
                    cmd.Parameters.AddWithValue("@id", blogPost.ID);
                    cmd.Parameters.AddWithValue("@title", blogPost.Title);
                    cmd.Parameters.AddWithValue("@post", blogPost.Post);
                    cmd.Parameters.AddWithValue("@created", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                    return blogPost;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void AddComment(BlogComment blogComment)
        {
            using (var connection = new SQLiteConnection(_conStr.ConnectionString))
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.Transaction = trans;

                    cmd.CommandText = _getMaxCommentId;
                    var maxId = cmd.ExecuteScalar();
                    blogComment.ID = maxId is DBNull ? 1 : Convert.ToInt32(maxId) + 1;

                    cmd.CommandText = _insertComment;
                    cmd.Parameters.AddWithValue("@id", blogComment.ID);
                    cmd.Parameters.AddWithValue("@postId", blogComment.PostId);
                    cmd.Parameters.AddWithValue("@comment", blogComment.Comment);
                    cmd.Parameters.AddWithValue("@created", blogComment.Created);
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public BlogPost GetBlogPost(int id)
        {
            using (var connection = new SQLiteConnection(_conStr.ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = _getBlog;
                cmd.Parameters.AddWithValue("@id", id);
                var bpData = cmd.ExecuteReader();

                if (!bpData.HasRows)
                {
                    throw new ApplicationException("No blog post found for id " + id);
                }

                bpData.Read();
                var blogPost = new BlogPost()
                {
                    ID = (int)bpData["id"],
                    Title = (string)bpData["title"],
                    Post = (string)bpData["post"],
                    Created = (DateTime)bpData["created"]
                };
                bpData.NextResult();
                while (bpData.Read())
                {
                    var blogComment = new BlogComment()
                    {
                        PostId = id,
                        Comment = (string)bpData["comment"],
                        Created = (DateTime)bpData["created"],
                        ID = (int)bpData["id"]
                    };
                    blogPost.Comments.Add(blogComment);
                }
                bpData.Close();

                return blogPost;
            }
        }

        public IList<BlogPost> GetBlogPosts(int startNum, int numToReturn)
        {
            using (var connection = new SQLiteConnection(_conStr.ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = _getBlogs;
                var blogData = cmd.ExecuteReader();
                var blogList = new List<BlogPost>();
                while (blogData.Read())
                {
                    var blogPost = new BlogPost()
                    {
                        ID = (int)blogData["id"],
                        Title = (string)blogData["title"],
                        Post = (string)blogData["post"],
                        Created = (DateTime)blogData["created"]
                    };
                    blogList.Add(blogPost);
                }

                return blogList;
            }
        }
    }
}
