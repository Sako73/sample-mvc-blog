using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModels.Models
{
    public class BlogPost
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Post { get; set; }
        public DateTime Created { get; set; }
        public IList<BlogComment> Comments { get; set; } = new List<BlogComment>();
    }
}