using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconTask.Response.BlogPost
{
    public class BlogPostList
    {
        public List<BlogPostDto> BlogPostDtos { get; set; }
        public string NumberOfPosts { get; set; }
    }
}
