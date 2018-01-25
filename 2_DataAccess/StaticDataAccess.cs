using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StaticDataAccess : IDataAccess
    {
        static IList<BlogPost> blogList;
        // A static constructor to make sure it persists until restarted.
        static StaticDataAccess()
        {
            blogList = CreateStaticBlog();
        }

        public IList<BlogPost> GetBlogPosts(int startNum, int numToReturn)
        {
            return blogList;
        }

        public BlogPost GetBlogPost(int id)
        {
            // Returns null if not found.
            var blogPost = blogList
                .Where(b => b.ID == id)
                .SingleOrDefault();

            if (blogPost == null)
            {
                throw new ApplicationException("Blog post not found");
            }

            blogPost.Comments = blogPost.Comments
                .OrderBy(c => c.Created)
                .ToList();

            return blogPost;
        }

        public void AddComment(BlogComment blogComment)
        {
            // Returns null if not found.
            var blogPost = blogList
                .Where(b => b.ID == blogComment.PostId)
                .SingleOrDefault();

            if (blogPost == null)
            {
                throw new ApplicationException("Blog post not found");
            }
            // Get the next id.
            int maxId = blogList
                .SelectMany(b => b.Comments)
                .Max(c => c.ID);
            blogComment.ID = maxId + 1;
            blogPost.Comments.Add(blogComment);
        }

        public BlogPost AddBlogPost(BlogPost blogPost)
        {
            // Make sure we don't have duplicate titles.
            if (blogList.Any(b => b.Title.ToUpper() == blogPost.Title.ToUpper()))
            {
                throw new ApplicationException("Duplicate blog titles");
            }

            // Get the next id.
            int maxId = blogList.Max(b => b.ID);

            blogPost.ID = maxId + 1;
            blogPost.Created = DateTime.Now;

            blogList.Add(blogPost);

            return blogPost;
        }

        // So unit tests can get a clean copy in each test.
        public void Initialize()
        {
            blogList = CreateStaticBlog();
        }
        private static IList<BlogPost> CreateStaticBlog()
        {
            IList<BlogPost> blogPosts = new List<BlogPost>();

            var blogPost = new BlogPost()
            {
                ID = 1,
                Title = "This is the first blog post",
                Post = loremTxt,
                Created = DateTime.Now.AddDays(-2),
                Comments = new List<BlogComment>()
                {
                    new BlogComment(){PostId=1, Comment="This is the first comment", ID = 1, Created = DateTime.Now.AddDays(-1)},
                    new BlogComment(){PostId=1, Comment=string.Concat(Enumerable.Repeat("This is a longer comment ", 9)), ID = 2, Created = DateTime.Now.AddHours(-12)},
                    new BlogComment(){PostId=1, Comment="Another comment", ID = 3, Created = DateTime.Now.AddHours(-8)},
                    new BlogComment(){PostId=1, Comment="I make $10,000 per week, find out how at www.scamme.com", ID = 4, Created = DateTime.Now.AddHours(-18)}
                }
            };
            blogPosts.Add(blogPost);

            blogPost = new BlogPost()
            {
                ID = 2,
                Title = "This is the second blog post",
                Post = loremTxt,
                Created = DateTime.Now.AddDays(-1),
                Comments = new List<BlogComment>()
                {
                    new BlogComment(){PostId=2, Comment="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec tincidunt lectus est, vel interdum erat egestas eu. Proin sit amet metus ac ipsum fringilla ultrices. Phasellus congue feugiat leo. Donec sed ornare quam. Nunc mollis placerat massa. Sed consequat fermentum pulvinar. Curabitur id nulla non est tempus auctor suscipit ac libero. Maecenas non pharetra odio.", ID = 5, Created = DateTime.Now.AddHours(-12)},
                    new BlogComment(){PostId=2, Comment="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec tincidunt lectus est, vel interdum erat egestas eu. Proin sit amet metus ac ipsum fringilla ultrices. Phasellus congue feugiat leo. Donec sed ornare", ID = 6, Created = DateTime.Now.AddHours(-8)}
                }
            };
            blogPosts.Add(blogPost);

            return blogPosts;
        }

        public const string loremTxt = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id hendrerit libero, vel egestas enim. Vestibulum pretium id magna ut ullamcorper. Cras ornare ut dui ac ultricies. Nunc vulputate lorem elementum ipsum hendrerit, id accumsan libero pharetra. Nam pellentesque velit et dolor laoreet euismod. Quisque hendrerit commodo magna, ac interdum libero suscipit id. Quisque mauris ligula, tincidunt et odio non, volutpat vulputate nunc. Phasellus sit amet vehicula nulla, nec ornare odio. Nam eget velit quis sem eleifend dapibus id laoreet leo. Etiam porttitor finibus libero sit amet mollis. Maecenas eu sollicitudin velit. Sed iaculis maximus tellus, eu luctus dolor ultricies quis. Etiam finibus, eros eu euismod commodo, orci nibh volutpat lectus, a porttitor dolor felis non ipsum. Fusce consequat in eros eget consectetur. Nunc finibus sem vel odio porta maximus. 
Donec arcu urna, consequat quis sem eu, vehicula eleifend libero.Maecenas rutrum erat feugiat nisl egestas, et imperdiet nisi tempus. Pellentesque in suscipit felis. Suspendisse hendrerit nec erat eget hendrerit. Aliquam feugiat mi venenatis ante molestie pulvinar consectetur ut augue. Mauris in sagittis est, ac fermentum metus.Sed lacinia auctor ante et ultricies. Nulla ut sapien condimentum, placerat nibh a, aliquet tellus. Maecenas sodales est sed odio venenatis, et scelerisque mauris rhoncus. In cursus maximus pulvinar. Cras convallis sit amet augue sed porta.Pellentesque sit amet tincidunt mauris, vitae fringilla turpis.Praesent lorem augue, elementum non nunc vel, malesuada facilisis ante.Donec purus ipsum, congue ut justo at, vestibulum posuere velit.Aliquam aliquet venenatis tempor. 
Aliquam elementum gravida pellentesque. Ut sed dolor nibh. Cras sed nunc in orci mattis congue vitae sed metus. Ut nec dolor interdum, aliquet erat sed, fermentum justo. Aenean rutrum libero at dignissim rutrum. Donec non diam vel libero dictum eleifend.Quisque in iaculis neque, vitae vestibulum turpis.In varius libero et ex efficitur suscipit eu sed arcu. Sed faucibus pellentesque ligula. Curabitur accumsan, neque id mattis fermentum, nulla felis semper felis, sit amet luctus felis augue at quam.Duis nulla arcu, tincidunt nec nulla non, fringilla volutpat ipsum.
Praesent scelerisque at est vitae hendrerit. Quisque pellentesque convallis diam sed feugiat. Nulla efficitur lobortis augue, non ornare nibh consectetur at.Nullam pulvinar turpis dui, quis vulputate odio blandit id.Nam quis neque quis leo ultrices posuere.Quisque venenatis facilisis diam ut posuere. Sed et justo vitae purus sodales pretium.Suspendisse ultrices sem vel mauris sagittis accumsan.Praesent bibendum quam arcu, nec elementum felis mattis non.
Quisque pretium augue ligula, nec placerat diam tempor sed.Pellentesque vitae hendrerit ante. Curabitur scelerisque pharetra orci, et suscipit neque auctor aliquet.Phasellus eget fringilla nulla, eu porttitor eros.Integer id velit quam. Aenean id orci ultrices, fringilla eros quis, rutrum quam. Cras viverra risus in mauris varius malesuada.Fusce nunc eros, porta eget enim in, faucibus sagittis ligula.In facilisis sem in quam placerat, a tincidunt sem lobortis. Sed pellentesque massa at justo tempor dignissim sit amet non tellus. ";
    }
}
