using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconTask.Request
{
    public class UpdateBlogPost
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
    }
}
