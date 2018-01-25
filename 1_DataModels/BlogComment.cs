using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class BlogComment
    {
        public int ID { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
