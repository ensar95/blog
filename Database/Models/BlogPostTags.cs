using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconTask.Database.Models
{
    public class BlogPostTags
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
